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

        public byte[] GetSalt
        {
            get
            {
                return File.ReadAllBytes(this.SaltFilePath);
            }
        }

        private string CreateWorkDirectoryName
        {
            get
            {
                var rndText = Path.GetRandomFileName().Replace(".", string.Empty);
                var dnt = DateTime.Now.ToString("yyyyMMdd_HHmmss_fffff");
                var result = $"Work__{dnt}__{rndText}";

                return result;
            }
        }

        public XModel.CryptoXFilePath GetCryptoWorkPath
        {
            get
            {
                var tempDirPath = this.CryptoTempDirectoryPath;
                var workDirName = this.CreateWorkDirectoryName;
                var workDirPath = this.CreateXDirectoryPath(true, tempDirPath, workDirName);
                var result = new XModel.CryptoXFilePath(workDirPath);

                return result;
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
