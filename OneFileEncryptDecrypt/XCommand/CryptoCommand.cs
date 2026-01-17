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

        private static Option<string> CreateOptionPassword(bool isEncryptCommand, XAppSettings.AppSettingsX asx)
        {
            var result = new Option<string>("--password", "-pw");
            // 암호화 비밀번호
            result.Description = asx.WorkMessage.CryptoPasswordDescription(isEncryptCommand);
            result.Required = true;
            result.Validators.Add(optr => CryptoCommand.CreateOptionKeyValidator(optr, asx));

            return result;
        }

        private static void CreateOptionKeyValidator(OptionResult optr, XAppSettings.AppSettingsX asx)
        {
            // 비밀번호 최소한의 길이
            var keyMinLength = XValue.ProcessValue.CryptoPasswordMinimumLength;
            var tkText = CommandProcess.IdentifierTokenText(optr);
            var key = optr.GetValueOrDefault<string>();
            // 비밀번호 길이는 일정길이 이상 필수로 잡음
            var isAllowKeyLen = ((key != string.Empty) && (key.Length >= keyMinLength));

            if (isAllowKeyLen == false)
            {
                // 비밀번호는 최소 X자 이상이어야 합니다.
                optr.AddError(asx.WorkMessage.CryptoPasswordNotAllowLength(tkText, keyMinLength));
            }
        }

        private static Option<string> CreateOptionFile(bool isEncryptCommand, XAppSettings.AppSettingsX asx)
        {
            var result = new Option<string>("--file", "-f");
            // 암호화 파일 경로
            result.Description = asx.WorkMessage.CryptoFileDescription(isEncryptCommand);
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

        public static Command CreateCommand(string commandName, Action<XAppSettings.AppSettingsX, XConsole.ConsoleWriteMessageSet, XModel.CryptoWorkOrder> workAction, XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            var isEncryptCommand = (commandName == CryptoCommand.EncryptCommandName);
            var optPW = CryptoCommand.CreateOptionPassword(isEncryptCommand, asx);
            var optFile = CryptoCommand.CreateOptionFile(isEncryptCommand, asx);
            // 파일을 암호화 합니다.
            var cmdDesc = asx.WorkMessage.CryptoCommandDescription(isEncryptCommand);

            var result = new Command(commandName, cmdDesc);
            result.Options.Add(optPW);
            result.Options.Add(optFile);

            result.SetAction(
                (ParseResult pr) =>
                {
                    // Salt 파일은 필수로 있어야 한다!
                    if (asx.Crypto.IsExistSaltFile == true)
                    {
                        var cryptoPW = (pr.GetValue(optPW) ?? string.Empty);
                        var filePath = (pr.GetValue(optFile) ?? string.Empty);
                        var cryptoMode = XValue.ProcessValue.CryptoMode_AESCBC;
                        var cwo = new XModel.CryptoWorkOrder(cryptoPW, filePath, cryptoMode);

                        Console.WriteLine(cryptoMode);

                        //workAction(asx, cwms, cwo);
                    }
                }
            );

            return result;
        }
    }
}
