using System.CommandLine;

using Microsoft.Extensions.Configuration;

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
                        errorMessage.Add($"Not exist crypto salt. Please run {cmdName}.");
                    }
                }
            }
            else
            {
                errorMessage.Add("Empty or wrong AppSetting.");
            }

            if (errorMessage.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine(string.Join(Environment.NewLine, errorMessage));
                Console.Out.WriteLine(string.Empty);
                Console.ResetColor();
            }
        }

        public static void Main(string[] args)
        {
            var asx = Program.ASX;

            Program.AppSettingDefaultCheck(asx, args);

            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/syntax
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/how-to-parse-and-invoke
            var rc = new RootCommand("One file encrypt and decrypt work.");
            rc.Add(XCommand.CryptoCommand.CreateCommand("encrypt", "Encrypt", XWork.EncryptWork.ExecuteNow));
            rc.Add(XCommand.CryptoCommand.CreateCommand("decrypt", "Decrypt", XWork.DecryptWork.ExecuteNow));
            rc.Add(XCommand.CreateSaltCommand.CreateCommand(XWork.CreateSaltWork.ExecuteNow));

            var pr = rc.Parse(args);
            //var pr = rc.Parse("encrypt --key helloworld --file D:\\Download\\Dummy\\IMG_2819.jpg");
            //var pr = rc.Parse("encrypt -k helloworld -f D:\\Download\\Dummy\\Hello.txt");

            pr.Invoke();
        }
    }
}



