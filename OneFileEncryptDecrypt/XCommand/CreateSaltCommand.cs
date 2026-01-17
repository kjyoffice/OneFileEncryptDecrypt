using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class CreateSaltCommand
    {
        public static Command CreateCommand(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            // 암호화, 복호화 Salt를 생성합니다.
            var description = asx.WorkMessage.CreateSaltDescription;
            var result = new Command("createsalt", description);

            result.SetAction(
                (ParseResult pr) =>
                {
                    XWork.CreateSaltWork.ExecuteNow(asx, cwms);
                }
            );

            return result;
        }
    }
}
