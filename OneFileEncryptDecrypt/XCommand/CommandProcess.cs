using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XCommand
{
    public class CommandProcess
    {
        public static string IdentifierTokenText(OptionResult optr)
        {
            var tk = optr.IdentifierToken;
            var result = ((tk != null) ? $"[{tk.Type} '{tk.Value}'] " : string.Empty);

            return result;
        }
    }
}
