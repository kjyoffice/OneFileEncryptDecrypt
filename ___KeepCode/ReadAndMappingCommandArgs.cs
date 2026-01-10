using Microsoft.Extensions.Configuration;

namespace OneFileEncryptDecrypt
{
    public class Program
    {
        private static XModel.AppSettingsMain CreateWorkOrder()
        {
            var args = Environment.GetCommandLineArgs();
            var configX = new ConfigurationBuilder().AddJsonFile("appsettings.json", true).AddCommandLine(args).Build();
            var asmData = configX.Get<XModel_Json.AppSettingsMain_Json>();
            var result = new XModel.AppSettingsMain(asmData);

            return result;
        }

        public static void Main()
        {
            // OneFileEncryptDecrypt /work encrypt /key "abcde" /file "hello.jpg"
            var asm = Program.CreateWorkOrder();
        }
    }
}




namespace OneFileEncryptDecrypt.XType
{
    public enum CryptoWorkType
    {
        Unknown,
        Encrypt,
        Decrypt
    }
}






using Microsoft.Extensions.Configuration;

namespace OneFileEncryptDecrypt.XModel_Json
{
    public class AppSettingsMain_Json
    {
        [ConfigurationKeyName("work")]
        public string? CryptoType { get; set; }
        [ConfigurationKeyName("key")]
        public string? CryptoKey { get; set; }
        [ConfigurationKeyName("file")]
        public string? FilePath { get; set; }
    }
}










using Microsoft.Extensions.Configuration;

namespace OneFileEncryptDecrypt.XModel
{
    public class AppSettingsMain
    {
        public bool IsAllow { get; private set; }
        public XType.CryptoWorkType CryptoType { get; private set; }
        public string CryptoKey { get; private set; }
        public string FilePath { get; private set; }

        // --------------------------------------------------

        private XType.CryptoWorkType GetCryptoType(XModel_Json.AppSettingsMain_Json? jsonData)
        {
            var cryptoType = (jsonData?.CryptoType ?? string.Empty).ToUpper();
            var result = (((cryptoType != string.Empty) && (Enum.TryParse<XType.CryptoWorkType>(cryptoType, true, out XType.CryptoWorkType wt) == true)) ? wt : XType.CryptoWorkType.Unknown);

            return result;
        }

        private bool IsAllowCheck(XType.CryptoWorkType cryptoType, string cryptoKey, string filePath)
        {
            return (
                ((cryptoType != XType.CryptoWorkType.Encrypt) || (cryptoType != XType.CryptoWorkType.Decrypt)) && 
                (cryptoKey != string.Empty) &&
                ((filePath != string.Empty) && (File.Exists(filePath) == true))
            );
        }

        // --------------------------------------------------

        public AppSettingsMain(XModel_Json.AppSettingsMain_Json? jsonData)
        {
            var cryptoType = this.GetCryptoType(jsonData);
            var cryptoKey = (jsonData?.CryptoKey ?? string.Empty);
            var filePath = (jsonData?.FilePath ?? string.Empty);

            this.IsAllow = this.IsAllowCheck(cryptoType, cryptoKey, filePath);
            this.CryptoType = cryptoType;
            this.CryptoKey = cryptoKey;
            this.FilePath = filePath;
        }
    }
}





appsettings.json
{
}
























