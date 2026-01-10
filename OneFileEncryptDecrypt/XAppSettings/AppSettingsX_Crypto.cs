using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XAppSettings
{
    public class AppSettingsX_Crypto
    {
        public bool IsAllow { get; private set; }
        public string SaltDirectoryPath { get; private set; }
        public string SaltFilePath { get; private set; }

        // --------------------------------------------------------

        public bool IsExistSaltFile
        {
            get
            {
                return File.Exists(this.SaltFilePath);
            }
        }

        // --------------------------------------------------------

        private string CreateSaltDirectoryPath(bool isAllow, string saltDirPath)
        {
            var result = ((isAllow == true) ? Path.Combine(saltDirPath, "OFEDCryptoSalt") : string.Empty);

            if ((isAllow == true) && (Directory.Exists(result) == false))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        // --------------------------------------------------------

        public AppSettingsX_Crypto(XAppSettings_Json.AppSettingsX_Crypto_Json? jsonData)
        {
            var saltDirPath = (jsonData?.SaltDirectoryPath ?? string.Empty);
            var isAllow = ((saltDirPath != string.Empty) && (Directory.Exists(saltDirPath) == true));
            var saltDirPathUse = this.CreateSaltDirectoryPath(isAllow, saltDirPath);

            this.IsAllow = isAllow;
            this.SaltDirectoryPath = saltDirPathUse;
            this.SaltFilePath = Path.Combine(saltDirPathUse, "CryptoSalt.ofed");
        }
    }
}
