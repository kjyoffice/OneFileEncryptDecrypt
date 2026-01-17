using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class ExportSaltCommand
    {
        private static Option<string> CreateOptionDirectory(XAppSettings.AppSettingsX asx)
        {
            var result = new Option<string>("--directory", "-d");
            // 내보내기 디렉토리 경로
            result.Description = asx.WorkMessage.ExportDirectoryPath;
            result.Required = true;
            result.Validators.Add(optr => ExportSaltCommand.CreateOptionDirectoryValidator(optr, asx));

            return result;
        }

        private static void CreateOptionDirectoryValidator(OptionResult optr, XAppSettings.AppSettingsX asx)
        {
            var tkText = CommandProcess.IdentifierTokenText(optr);
            var dirPath = optr.GetValueOrDefault<string>();
            var isExist = ((dirPath != string.Empty) && (Directory.Exists(dirPath) == true));

            if (isExist == false)
            {
                // 내보내기 디렉토리가 존재하지 않습니다.
                optr.AddError(asx.WorkMessage.NotExistExportDirectory(tkText));
            }
        }

        // -------------------------------------------------------------------------------------------------

        public static Command CreateCommand(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            var optDirPath = ExportSaltCommand.CreateOptionDirectory(asx);
            // 암호화, 복호화 Salt를 내보냅니다.
            var description = asx.WorkMessage.ExportSaltDescription;
            var result = new Command("exportsalt", description);
            result.Options.Add(optDirPath);

            result.SetAction(
                (ParseResult pr) =>
                {
                    var dirPath = (pr.GetValue(optDirPath) ?? string.Empty);
                    var isExecute = (
                        (asx.Crypto.IsExistSaltFile == true) &&
                        ((dirPath != string.Empty) && (Directory.Exists(dirPath) == true))
                    );

                    if (isExecute == true)
                    {
                        XWork.ExportSaltWork.ExecuteNow(asx, cwms, dirPath);
                    }
                }
            );

            return result;
        }
    }
}
