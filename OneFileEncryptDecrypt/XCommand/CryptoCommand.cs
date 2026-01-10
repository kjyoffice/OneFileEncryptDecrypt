using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class CryptoCommand
    {
        private static Option<string> CreateOptionKey(string workText)
        {
            var result = new Option<string>("--key", "-k");
            result.Description = $"{workText} key";
            result.Required = true;
            result.Validators.Add(CryptoCommand.CreateOptionKeyValidator);

            return result;
        }

        private static void CreateOptionKeyValidator(OptionResult optr)
        {
            // 키 최소한의 길이
            var keyMinLength = XValue.ProcessValue.CryptoKeyMinimumLength;
            var tkText = CommandProcess.IdentifierTokenText(optr);
            var key = optr.GetValueOrDefault<string>();
            // 키 길이는 일정길이 이상 필수로 잡음
            var isAllowKeyLen = ((key != string.Empty) && (key.Length >= keyMinLength));

            if (isAllowKeyLen == false)
            {
                optr.AddError($"{tkText}Want length minimum {keyMinLength}.");
            }
        }

        private static Option<string> CreateOptionFile(string workText)
        {
            var result = new Option<string>("--file", "-f");
            result.Description = $"{workText} source file path";
            result.Required = true;
            result.Validators.Add(CryptoCommand.CreateOptionFileValidator);

            return result;
        }

        private static void CreateOptionFileValidator(OptionResult optr)
        {
            var tkText = CommandProcess.IdentifierTokenText(optr);
            var filePath = optr.GetValueOrDefault<string>();

            // 파일이 존재하는지 체크
            if ((filePath != string.Empty) && (File.Exists(filePath) == true))
            {
                var maxSizeMB = XValue.ProcessValue.FileAllowMaxSizeMB;
                // 1048576 : 1024 * 1024
                var maxByte = (1_048_576L * (maxSizeMB * 1L));
                var fi = new FileInfo(filePath);

                // 파일은 일정 크기 이상 안되게 한다
                if (fi.Length > maxByte)
                {
                    optr.AddError($"{tkText}Input file less {maxSizeMB} MB please.");
                }
            }
            else
            {
                optr.AddError($"{tkText}Not exist file.");
            }
        }

        // ----------------------------------------------------------------------------------------------------------

        public static Command CreateCommand(string workName, string workText, Action<XAppSettings.AppSettingsX, XModel.CryptoWorkOrder> workAction)
        {
            var asx = Program.ASX;
            var optKey = CryptoCommand.CreateOptionKey(workText);
            var optFile = CryptoCommand.CreateOptionFile(workText);

            var result = new Command(workName, $"{workText} a file");
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
