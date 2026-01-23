using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class CryptoCommand
    {
        private static Option<string> CreateOptionPassword(bool isEncrypt, XAppSettings.AppSettingsX asx)
        {
            var result = new Option<string>("--password", "-p");
            // 암호화 비밀번호
            result.Description = asx.WorkMessage.CryptoPasswordDescription(isEncrypt);
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

        private static Option<string> CreateOptionFile(bool isEncrypt, XAppSettings.AppSettingsX asx)
        {
            var result = new Option<string>("--file", "-f");
            // 암호화 파일 경로
            result.Description = asx.WorkMessage.CryptoFileDescription(isEncrypt);
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

        private static Option<string> CreateOptionMode(XAppSettings.AppSettingsX asx)
        {
            var result = new Option<string>("--mode", "-m");
            // 암호화 방법을 지정합니다.
            result.Description = asx.WorkMessage.CryptoModeDescription;
            result.Required = false;
            result.Validators.Add(optr => CryptoCommand.CreateOptionModeValidator(optr, asx));

            return result;
        }

        private static void CreateOptionModeValidator(OptionResult optr, XAppSettings.AppSettingsX asx)
        {
            var tkText = CommandProcess.IdentifierTokenText(optr);
            var cryptoMode = optr.GetValueOrDefault<string>().ToUpper();
            var isAllow = (
                (cryptoMode == string.Empty) ||
                ((cryptoMode != string.Empty) && (cryptoMode == XValue.ProcessValue.CryptoMode_AES256CBC))
            );

            if (isAllow == false)
            {
                // 지정되지 않은 Mode 입니다.
                optr.AddError(asx.WorkMessage.UndefinedMode(tkText));
            }
        }

        // ----------------------------------------------------------------------------------------------------------

        public static Command CreateCommand(string commandName, Action<XAppSettings.AppSettingsX, XConsole.ConsoleWriteMessageSet, XModel.CryptoWorkOrder> workAction, XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, bool isEncrypt)
        {
            var optPW = CryptoCommand.CreateOptionPassword(isEncrypt, asx);
            var optFile = CryptoCommand.CreateOptionFile(isEncrypt, asx);
            var optMode = CryptoCommand.CreateOptionMode(asx);
            // 파일을 암호화 합니다.
            var cmdDesc = asx.WorkMessage.CryptoCommandDescription(isEncrypt);

            var result = new Command(commandName, cmdDesc);
            result.Options.Add(optPW);
            result.Options.Add(optFile);

            if (isEncrypt == true)
            {
                result.Options.Add(optMode);
            }

            result.SetAction(
                (ParseResult pr) =>
                {
                    var cryptoPW = (pr.GetValue(optPW) ?? string.Empty);
                    var filePath = (pr.GetValue(optFile) ?? string.Empty);
                    var cryptoMode = (pr.GetValue(optMode) ?? string.Empty);
                    var cwo = new XModel.CryptoWorkOrder(cryptoPW, filePath, cryptoMode, isEncrypt);

                    workAction(asx, cwms, cwo);
                }
            );

            return result;
        }
    }
}
