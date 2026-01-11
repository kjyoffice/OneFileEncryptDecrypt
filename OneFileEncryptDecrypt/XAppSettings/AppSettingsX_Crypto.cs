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
        public string CryptoTempDirectoryPath { get; private set; }

        // --------------------------------------------------------

        public bool IsExistSaltFile
        {
            get
            {
                return File.Exists(this.SaltFilePath);
            }
        }

        // --------------------------------------------------------

        private string CreateXDirectoryPath(bool isAllow, string saltDirPath, string dirName)
        {
            var result = ((isAllow == true) ? Path.Combine(saltDirPath, dirName) : string.Empty);

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
            var saltDirPathUse = this.CreateXDirectoryPath(isAllow, saltDirPath, "OFEDCryptoSalt");

            this.IsAllow = isAllow;
            this.SaltDirectoryPath = saltDirPathUse;
            this.SaltFilePath = Path.Combine(saltDirPathUse, "CryptoSalt.ofed");
            this.CryptoTempDirectoryPath = this.CreateXDirectoryPath(isAllow, saltDirPath, "OFEDCryptoTemp_IfWantDeleteIsOK");
        }
    }
}
