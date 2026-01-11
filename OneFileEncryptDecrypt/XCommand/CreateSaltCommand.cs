using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class CreateSaltCommand
    {
        public static string CommandName
        {
            get
            {
                return "createsalt";
            }
        }

        // -------------------------------------------------------------------------

        public static Command CreateCommand(Action<XAppSettings.AppSettingsX, XConsole.ConsoleWriteMessageSet> workAction, XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            var cmdName = CreateSaltCommand.CommandName;
            // 암호화, 복호화 Salt를 생성합니다.
            var result = new Command(cmdName, asx.WorkMessage.CreateSaltDescription);

            result.SetAction(
                (ParseResult pr) =>
                {
                    workAction(asx, cwms);
                }
            );

            return result;
        }
    }
}
