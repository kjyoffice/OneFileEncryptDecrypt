using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class MainCommandWork
    {
        private static string IdentifierTokenText(OptionResult optr)
        {
            var tk = optr.IdentifierToken;
            var result = ((tk != null) ? $"[{tk.Type} '{tk.Value}'] " : string.Empty);

            return result;
        }

        private static void OptionValidator_Key(OptionResult optr)
        {
            var tkText = MainCommandWork.IdentifierTokenText(optr);
            var value = optr.GetValueOrDefault<string>();
            var isAllow = ((value != string.Empty) && (value.Length <= 32));

            if (isAllow == false)
            {
                optr.AddError($"{tkText}Want size 1 ~ 32 length.");
            }
        }

        private static void OptionValidator_File(OptionResult optr)
        {
            var tkText = MainCommandWork.IdentifierTokenText(optr);
            var value = optr.GetValueOrDefault<string>();
            var isAllow = ((value != string.Empty) && (File.Exists(value) == true));

            if (isAllow == false)
            {
                optr.AddError($"{tkText}Not exist file.");
            }
        }

        private static Option<string> CreateOptionKey(string workText)
        {
            var result = new Option<string>("--key", "-k");
            result.Description = $"{workText} key";
            result.Required = true;
            result.Validators.Add(MainCommandWork.OptionValidator_Key);

            return result;
        }

        private static Option<string> CreateOptionFile(string workText)
        {
            var result = new Option<string>("--file", "-f");
            result.Description = $"{workText} source file path";
            result.Required = true;
            result.Validators.Add(MainCommandWork.OptionValidator_File);

            return result;
        }

        public static Command CreateCommand(string workName, string workText, Action<XModel.WorkOrder> workAction)
        {
            var optKey = MainCommandWork.CreateOptionKey(workText);
            var optFile = MainCommandWork.CreateOptionFile(workText);

            var result = new Command(workName, $"{workText} a file");
            result.Options.Add(optKey);
            result.Options.Add(optFile);

            result.SetAction(
                (ParseResult pr) =>
                {
                    var cryptoKey = (pr.GetValue(optKey) ?? string.Empty);
                    var filePath = (pr.GetValue(optFile) ?? string.Empty);
                    var wo = new XModel.WorkOrder(cryptoKey, filePath);

                    workAction(wo);
                }
            );

            return result;
        }
    }
}
