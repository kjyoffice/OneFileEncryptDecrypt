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
        private static XAppSettings.AppSettingsX GetAppSetting()
        {
            var configX = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();
            var jsonData = configX.Get<XAppSettings_Json.AppSettingsX_Json>();
            var result = new XAppSettings.AppSettingsX(jsonData);

            return result;
        }

        private static void AppSettingDefaultCheck(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            var errorMessage = new List<string>();

            if (asx.IsAllow == true)
            {
                if (asx.Crypto.IsExistSaltFile == false)
                {
                    // 암호화 Salt가 없습니다.
                    errorMessage.AddRange(asx.WorkMessage.NotExistCryptoSalt);
                    errorMessage.Add(string.Empty);
                }
            }
            else
            {
                // AppSettings이 없거나 올바르지 않습니다.
                errorMessage.Add(asx.WorkMessage.EmptyOrWrongAppSettings);
                errorMessage.Add(string.Empty);
            }

            cwms.Error.MessageNow(errorMessage, true);
        }

        // -------------------------------------------------------------------------------

        // OneFileEncryptDecrypt encrypt -p 0123456789 -f d:\Download\Dummy\IMG_2819.JPG
        // OneFileEncryptDecrypt decrypt -p 0123456789 -f d:\Download\Dummy\IMG_2819.JPG.ofedx
        public static void Main(string[] args)
        {
            var asx = Program.GetAppSetting();
            var cwms = new XConsole.ConsoleWriteMessageSet();

            Program.AppSettingDefaultCheck(asx, cwms);

            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/syntax
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/how-to-parse-and-invoke
            // 하나의 파일을 암호화 및 복호화 합니다.
            var rc = new RootCommand(asx.WorkMessage.AppDescription);
            rc.Add(XCommand.CryptoCommand.CreateCommand("encrypt", XWork.EncryptWork.ExecuteNow, asx, cwms, true));
            rc.Add(XCommand.CryptoCommand.CreateCommand("decrypt", XWork.DecryptWork.ExecuteNow, asx, cwms, false));
            rc.Add(XCommand.CreateSaltCommand.CreateCommand(asx, cwms));
            rc.Add(XCommand.ImportSaltCommand.CreateCommand(asx, cwms));
            rc.Add(XCommand.ExportSaltCommand.CreateCommand(asx, cwms));

            var pr = rc.Parse(args);
            //var pr = rc.Parse("encrypt -p helloworld -f D:\\Download\\Dummy\\IMG_2819.jpg");
            //var pr = rc.Parse("decrypt -p helloworld -f D:\\Download\\Dummy\\IMG_2819.jpg.ofedx");

            pr.Invoke();
        }
    }
}



