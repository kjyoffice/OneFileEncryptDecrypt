using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class CryptoCommand
    {
        public static string EncryptCommandName
        {
            get
            {
                return "encrypt";
            }
        }

        public static string DecryptCommandName
        {
            get
            {
                return "decrypt";
            }
        }

        // -------------------------------------------------------------------

        private static Option<string> CreateOptionKey(string commandName, XAppSettings.AppSettingsX asx)
        {
            var result = new Option<string>("--key", "-k");
            // 암호화 키
            result.Description = asx.WorkMessage.CryptoKeyDescription(commandName);
            result.Required = true;
            result.Validators.Add(optr => CryptoCommand.CreateOptionKeyValidator(optr, asx));

            return result;
        }

        private static void CreateOptionKeyValidator(OptionResult optr, XAppSettings.AppSettingsX asx)
        {
            // 키 최소한의 길이
            var keyMinLength = XValue.ProcessValue.CryptoKeyMinimumLength;
            var tkText = CommandProcess.IdentifierTokenText(optr);
            var key = optr.GetValueOrDefault<string>();
            // 키 길이는 일정길이 이상 필수로 잡음
            var isAllowKeyLen = ((key != string.Empty) && (key.Length >= keyMinLength));

            if (isAllowKeyLen == false)
            {
                // 키는 최소 X자 이상이어야 합니다.
                optr.AddError(asx.WorkMessage.CryptoKeyNotAllowLength(tkText, keyMinLength));
            }
        }

        private static Option<string> CreateOptionFile(string commandName, XAppSettings.AppSettingsX asx)
        {
            var result = new Option<string>("--file", "-f");
            // 암호화 파일 경로
            result.Description = asx.WorkMessage.CryptoFileDescription(commandName);
            result.Required = true;
            result.Validators.Add(optr => CryptoCommand.CreateOptionFileValidator(optr, asx));

            return result;
        }

        private static void CreateOptionFileValidator(OptionResult optr, XAppSettings.AppSettingsX asx)
        {
            var tkText = CommandProcess.IdentifierTokenText(optr);
            var filePath = optr.GetValueOrDefault<string>();

            // 파일이 존재하는지 체크
            if ((filePath != string.Empty) && (File.Exists(filePath) == true))
            {
                var maxSizeMB = XValue.ProcessValue.FileAllowMaxSizeMB;
                // 1048576 : 1024 * 1024
                var maxByte = (1_048_576L * (maxSizeMB + 1));
                var fi = new FileInfo(filePath);

                // 파일은 일정 크기 이상 안되게 한다
                if (fi.Length > maxByte)
                {
                    // 100 MB 이상의 파일은 지원하지 않습니다.
                    optr.AddError(asx.WorkMessage.CryptoFileBigNotSupport(tkText, maxSizeMB));
                }
            }
            else
            {
                // 파일이 존재하지 않습니다.
                optr.AddError(asx.WorkMessage.CryptoFileNotExist(tkText));
            }
        }

        // ----------------------------------------------------------------------------------------------------------

        public static Command CreateCommand(string commandName, Action<XAppSettings.AppSettingsX, XModel.CryptoWorkOrder> workAction)
        {
            var asx = Program.ASX;
            var optKey = CryptoCommand.CreateOptionKey(commandName, asx);
            var optFile = CryptoCommand.CreateOptionFile(commandName, asx);
            // 파일을 암호화 합니다.
            var cmdDesc = asx.WorkMessage.CryptoCommandDescription(commandName);

            var result = new Command(commandName, cmdDesc);
            result.Options.Add(optKey);
            result.Options.Add(optFile);

            result.SetAction(
                (ParseResult pr) =>
                {
                    // Salt 파일은 필수로 있어야 한다!
                    if (asx.Crypto.IsExistSaltFile == true)
                    {
                        var cryptoKey = (pr.GetValue(optKey) ?? string.Empty);
                        var filePath = (pr.GetValue(optFile) ?? string.Empty);
                        var cwo = new XModel.CryptoWorkOrder(cryptoKey, filePath);

                        workAction(asx, cwo);
                    }
                }
            );

            return result;
        }
    }
}
