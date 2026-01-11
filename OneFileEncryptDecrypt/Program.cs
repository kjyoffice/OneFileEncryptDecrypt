using System.CommandLine;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace OneFileEncryptDecrypt
{
    public class Program
    {
        public static XAppSettings.AppSettingsX ASX { get; private set; } = Program.GetAppSetting();

        // -------------------------------------------------------------------------------

        private static XAppSettings.AppSettingsX GetAppSetting()
        {
            var configX = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();
            var jsonData = configX.Get<XAppSettings_Json.AppSettingsX_Json>();
            var result = new XAppSettings.AppSettingsX(jsonData);

            return result;
        }

        private static void AppSettingDefaultCheck(XAppSettings.AppSettingsX asx, string[] args)
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

            Program.ErrorMessage(errorMessage, true);
        }

        private static void ConsoleWriteMessage(ConsoleColor textColor, bool isAndEmptyLine, params string[] message)
        {
            if (message.Length > 0)
            {
                Console.ForegroundColor = textColor;
                Console.Out.WriteLine(string.Join(Environment.NewLine, message));

                if (isAndEmptyLine == true)
                {
                    Console.Out.WriteLine(string.Empty);
                }

                Console.ResetColor();
            }
        }

        // -------------------------------------------------------------------------------

        public static void ErrorMessage(string message, bool isAndEmptyLine)
        {
            Program.ConsoleWriteMessage(ConsoleColor.Red, isAndEmptyLine, message);
        }

        public static void ErrorMessage(List<string> message, bool isAndEmptyLine)
        {
            Program.ConsoleWriteMessage(ConsoleColor.Red, isAndEmptyLine, message.ToArray());
        }

        public static void WarningMessage(string message, bool isAndEmptyLine)
        {
            Program.ConsoleWriteMessage(ConsoleColor.Yellow, isAndEmptyLine, message);
        }

        public static void WarningMessage(List<string> message, bool isAndEmptyLine)
        {
            Program.ConsoleWriteMessage(ConsoleColor.Yellow, isAndEmptyLine, message.ToArray());
        }

        public static void SuccessMessage(string message, bool isAndEmptyLine)
        {
            Program.ConsoleWriteMessage(ConsoleColor.Green, isAndEmptyLine, message);
        }

        public static void SuccessMessage(List<string> message, bool isAndEmptyLine)
        {
            Program.ConsoleWriteMessage(ConsoleColor.Green, isAndEmptyLine, message.ToArray());
        }

        public static void Main(string[] args)
        {
            var asx = Program.ASX;

            Program.AppSettingDefaultCheck(asx, args);

            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/syntax
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/how-to-parse-and-invoke
            // 하나의 파일을 암호화 및 복호화 합니다.
            var rc = new RootCommand(asx.WorkMessage.AppDescription);
            rc.Add(XCommand.CryptoCommand.CreateCommand(XCommand.CryptoCommand.EncryptCommandName, XWork.EncryptWork.ExecuteNow));
            rc.Add(XCommand.CryptoCommand.CreateCommand(XCommand.CryptoCommand.DecryptCommandName, XWork.DecryptWork.ExecuteNow));
            rc.Add(XCommand.CreateSaltCommand.CreateCommand(XWork.CreateSaltWork.ExecuteNow));

            var pr = rc.Parse(args);
            //var pr = rc.Parse("encrypt --key helloworld --file D:\\Download\\Dummy\\IMG_2819.jpg");
            //var pr = rc.Parse("encrypt -k helloworld -f D:\\Download\\Dummy\\Hello.txt");

            pr.Invoke();
        }
    }
}



