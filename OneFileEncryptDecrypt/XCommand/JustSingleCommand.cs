using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class JustSingleCommand
    {
        public static Command CreateCommand(string cmdName, string description, Action<XAppSettings.AppSettingsX, XConsole.ConsoleWriteMessageSet> workAction, XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            var result = new Command(cmdName, description);

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
