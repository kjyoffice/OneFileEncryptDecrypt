using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class ExportSaltCommand
    {
        public static Command CreateCommand(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            // 암호화, 복호화 Salt를 내보냅니다.
            var description = asx.WorkMessage.ExportSaltDescription;
            var result = new Command("exportsalt", description);

            result.SetAction(
                (ParseResult pr) =>
                {
                    XWork.ExportSaltWork.ExecuteNow(asx, cwms);
                }
            );

            return result;
        }
    }
}
