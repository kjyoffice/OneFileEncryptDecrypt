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

        private static void AppSettingDefaultCheck(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, string[] args)
        {
            var errorMessage = new List<string>();

            if (asx.IsAllow == true)
            {
                if (asx.Crypto.IsExistSaltFile == false)
                {
                    var cmdName = XCommand.CreateSaltCommand.CommandName;

                    if ((args.Length <= 0) || ((args.Length > 0) && (args[0].ToLower() != cmdName)))
                    {
                        // 암호화 Salt가 없습니다. 다음의 명령을 실행해주세요.
                        errorMessage.AddRange(asx.WorkMessage.NotExistCryptoSalt(cmdName));
                    }
                }
            }
            else
            {
                // AppSettings이 없거나 올바르지 않습니다.
                errorMessage.Add(asx.WorkMessage.EmptyOrWrongAppSettings);
            }

            cwms.Error.MessageNow(errorMessage, true);
        }

        // -------------------------------------------------------------------------------

        // OneFileEncryptDecrypt encrypt -pw 0123456789 -f d:\Download\Dummy\IMG_2819.JPG
        // OneFileEncryptDecrypt decrypt -pw 0123456789 -f d:\Download\Dummy\IMG_2819.JPG.ofedx
        public static void Main(string[] args)
        {
            var asx = Program.GetAppSetting();
            var cwms = new XConsole.ConsoleWriteMessageSet();

            Program.AppSettingDefaultCheck(asx, cwms, args);

            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/syntax
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/how-to-parse-and-invoke
            // 하나의 파일을 암호화 및 복호화 합니다.
            var rc = new RootCommand(asx.WorkMessage.AppDescription);
            rc.Add(XCommand.CryptoCommand.CreateCommand(XCommand.CryptoCommand.EncryptCommandName, XWork.EncryptWork.ExecuteNow, asx, cwms));
            rc.Add(XCommand.CryptoCommand.CreateCommand(XCommand.CryptoCommand.DecryptCommandName, XWork.DecryptWork.ExecuteNow, asx, cwms));
            rc.Add(XCommand.CreateSaltCommand.CreateCommand(XWork.CreateSaltWork.ExecuteNow, asx, cwms));

            var pr = rc.Parse(args);
            //var pr = rc.Parse("encrypt --key helloworld --file D:\\Download\\Dummy\\IMG_2819.jpg");
            //var pr = rc.Parse("encrypt -k helloworld -f D:\\Download\\Dummy\\Hello.txt");

            pr.Invoke();

            // AES-GCM
            /*
            var pw = Encoding.UTF8.GetBytes("hello");
            var salt = Encoding.UTF8.GetBytes("world");
            var key = XCrypto.AES256Process.CreateKey(pw, salt);
            var nonce = XCrypto.AES256Process.CreateNonce();
            var plainText = Encoding.UTF8.GetBytes("Hello World");
            var aad = Encoding.UTF8.GetBytes("JSON OR TEXT, Want non encrypt data like Header, Info!"); // Optional

            var encryptX = new byte[plainText.Length];
            var tagX = new byte[16];

            // https://learn.microsoft.com/ko-kr/dotnet/api/system.security.cryptography.aesgcm?view=net-10.0
            // https://www.scottbrady.io/c-sharp/aes-gcm-dotnet
            using (var aesGcm = new AesGcm(key, tagX.Length))
            {
                aesGcm.Encrypt(
                    nonce,
                    plainText,
                    encryptX,
                    tagX,
                    aad
                );
            }

            // Save!
            // nonce
            // encryptX
            // tagX

            var decryptX = new byte[encryptX.Length];

            using (var aesGcm = new AesGcm(key, tagX.Length))
            {
                aesGcm.Decrypt(
                    nonce,
                    encryptX,
                    tagX,
                    decryptX,
                    aad
                );
            }
            // catch (CryptographicException)

            Console.WriteLine(Encoding.UTF8.GetString(plainText));
            Console.WriteLine(Encoding.UTF8.GetString(decryptX));
            */
        }
    }
}



