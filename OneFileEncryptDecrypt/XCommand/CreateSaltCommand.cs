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

        public static Command CreateCommand(Action<XAppSettings.AppSettingsX> workAction)
        {
            var asx = Program.ASX;
            var cmdName = CreateSaltCommand.CommandName;
            var result = new Command(cmdName, $"Create crypto salt.");

            result.SetAction(
                (ParseResult pr) =>
                {
                    workAction(asx);
                }
            );

            return result;
        }
    }
}
