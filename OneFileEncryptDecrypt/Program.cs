using Microsoft.Extensions.Configuration;

namespace OneFileEncryptDecrypt
{
    public class Program
    {
        public static void Main()
        {
            var args = Environment.GetCommandLineArgs();
            var configX = new ConfigurationBuilder().AddJsonFile("appsettings.json", true).AddCommandLine(args).Build();
            var xx = configX.Get<XAppSettings_Json.AppSettingsMain_Json>();

            Console.Out.WriteLine(xx?.WorkID);
            Console.Out.WriteLine(xx?.Say);

            Console.Out.WriteLine("Hello, World!");
        }
    }
}
