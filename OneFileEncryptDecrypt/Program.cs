using System.CommandLine;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace OneFileEncryptDecrypt
{
    public class Program
    {
        private static bool IsExecuteArgsEnableUIXMode(string[] args)
        {
            var argsUse = (new List<string>(args)).Concat(Enumerable.Range(0, 3).Select(x => string.Empty)).Select(x => x.ToUpper()).ToList();
            var result = false;

            for (var i = 0; i < argsUse.Count; i++)
            {
                if ((argsUse[i] == "--ISUIX") && (argsUse[(i + 1)] == "TRUE"))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        // ----------------------------------------------------------------

        // OneFileEncryptDecrypt encrypt -p 0123456789 -f d:\Download\Dummy\IMG_2819.JPG
        // OneFileEncryptDecrypt decrypt -p 0123456789 -f d:\Download\Dummy\IMG_2819.JPG.ofedx
        public static void Main(string[] args)
        {
            var isUIXMode = Program.IsExecuteArgsEnableUIXMode(args);
            var asx = new XAppSettings.AppSettingsX(isUIXMode);
            var cwms = new XConsole.ConsoleWriteMessageSet();

            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/syntax
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/how-to-parse-and-invoke
            // 하나의 파일을 암호화 및 복호화 합니다.
            var rc = new RootCommand(asx.WorkMessage.AppDescription);
            rc.Add(XCommand.CryptoCommand.CreateCommand("encrypt", XWork.EncryptWork.ExecuteNow, asx, cwms, true));
            rc.Add(XCommand.CryptoCommand.CreateCommand("decrypt", XWork.DecryptWork.ExecuteNow, asx, cwms, false));

            var pr = rc.Parse(args);
            //var pr = rc.Parse("encrypt -p helloworld -f D:\\Download\\Dummy\\IMG_2819.jpg");
            //var pr = rc.Parse("decrypt -p helloworld -f D:\\Download\\Dummy\\IMG_2819.jpg.ofedx");

            pr.Invoke();

            if ((asx.IsUIXMode == true) && (asx.IsFinalSuccess == false))
            {
                Console.Out.WriteLine(string.Empty);
                // 프로그램 종료를 위해서는 Enter를 쳐주세요.
                Console.Out.WriteLine(asx.WorkMessage.ProgramExitPressEnter);
                Console.In.ReadLine();
            }
        }
    }
}



