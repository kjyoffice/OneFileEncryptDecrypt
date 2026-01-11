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

        private string CreateXDirectoryPath(bool isAllow, string dirPath, string dirName)
        {
            var dirPathUse = ((isAllow == true) ? Path.Combine(dirPath, dirName) : string.Empty);

            if ((isAllow == true) && (Directory.Exists(dirPathUse) == false))
            {
                Directory.CreateDirectory(dirPathUse);
            }

            var result = ((isAllow == true) ? Path.GetFullPath(dirPathUse) : string.Empty);

            return result;
        }

        // --------------------------------------------------------

        public AppSettingsX_Crypto(XAppSettings_Json.AppSettingsX_Crypto_Json? jsonData)
        {
            var workDirPath = (jsonData?.WorkDirectoryPath ?? string.Empty);
            var isAllow = ((workDirPath != string.Empty) && (Directory.Exists(workDirPath) == true));
            var saltDirPathUse = this.CreateXDirectoryPath(isAllow, workDirPath, "OFEDCryptoSalt");

            this.IsAllow = isAllow;
            this.SaltDirectoryPath = saltDirPathUse;
            this.SaltFilePath = Path.Combine(saltDirPathUse, "CryptoSalt.ofed");
            this.CryptoTempDirectoryPath = this.CreateXDirectoryPath(isAllow, workDirPath, "OFEDCryptoTemp_IfWantDeleteIsOK");
        }
    }
}
