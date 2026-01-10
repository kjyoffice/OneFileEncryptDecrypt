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
            // 키 최소한의 길이
            var keyMinLength = XValue.ProcessValue.CryptoKeyMinimumLength;
            var tkText = MainCommandWork.IdentifierTokenText(optr);
            var key = optr.GetValueOrDefault<string>();
            // 키 길이는 일정길이 이상 필수로 잡음
            var isAllowKeyLen = ((key != string.Empty) && (key.Length >= keyMinLength));

            if (isAllowKeyLen == false)
            {
                optr.AddError($"{tkText}Want length minimum {keyMinLength}.");
            }
        }

        private static void OptionValidator_File(OptionResult optr)
        {
            var maxSizeMB = XValue.ProcessValue.FileAllowMaxSizeMB;
            var tkText = MainCommandWork.IdentifierTokenText(optr);
            var filePath = optr.GetValueOrDefault<string>();

            // 파일이 존재하는지 체크
            if ((filePath != string.Empty) && (File.Exists(filePath) == true))
            {
                var fi = new FileInfo(filePath);
                var maxByte = (1_024_000L * (maxSizeMB * 1L));

                // 파일은 일정 크기 이상 안되게 한다
                if (fi.Length > maxByte)
                {
                    optr.AddError($"{tkText}Input file less {maxSizeMB} MB please.");
                }
            }
            else
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
