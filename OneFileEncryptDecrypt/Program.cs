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
        // OneFileEncryptDecrypt encrypt -p 0123456789 -f d:\Download\Dummy\IMG_2819.JPG
        // OneFileEncryptDecrypt decrypt -p 0123456789 -f d:\Download\Dummy\IMG_2819.JPG.ofedx
        public static void Main(string[] args)
        {
            var asx = new XAppSettings.AppSettingsX();
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
        }
    }
}



