using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class ImportSaltCommand
    {
        private static Option<string> CreateOptionFile(XAppSettings.AppSettingsX asx)
        {
            var result = new Option<string>("--file", "-f");
            // 가져오기 파일 경로
            result.Description = asx.WorkMessage.ImportFilePath;
            result.Required = true;
            result.Validators.Add(optr => ImportSaltCommand.CreateOptionFileValidator(optr, asx));

            return result;
        }

        private static void CreateOptionFileValidator(OptionResult optr, XAppSettings.AppSettingsX asx)
        {
            var tkText = CommandProcess.IdentifierTokenText(optr);
            var filePath = optr.GetValueOrDefault<string>();
            var isExistFile = ((filePath != string.Empty) && (File.Exists(filePath) == true));

            // 파일이 존재하는지 체크
            if (isExistFile == false)
            {
                // 파일이 존재하지 않습니다.
                optr.AddError(asx.WorkMessage.CryptoFileNotExist(tkText));
            }
        }

        // ----------------------------------------------------------------------------------------

        public static Command CreateCommand(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            var optFile = ImportSaltCommand.CreateOptionFile(asx);
            // 암호화, 복호화 Salt를 가져옵니다.
            var description = asx.WorkMessage.ImportSaltDescription;
            var result = new Command("importsalt", description);
            result.Options.Add(optFile);

            result.SetAction(
                (ParseResult pr) =>
                {
                    var filePath = (pr.GetValue(optFile) ?? string.Empty);

                    XWork.ImportSaltWork.ExecuteNow(asx, cwms, filePath);
                }
            );

            return result;
        }
    }
}
