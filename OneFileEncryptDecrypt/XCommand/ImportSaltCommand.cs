using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class ImportSaltCommand
    {
        public static Command CreateCommand(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            // 암호화, 복호화 Salt를 가져옵니다.
            var description = asx.WorkMessage.ImportSaltDescription;
            var result = new Command("importsalt", description);

            result.SetAction(
                (ParseResult pr) =>
                {
                    XWork.ImportSaltWork.ExecuteNow(asx, cwms);
                }
            );

            return result;
        }
    }
}
