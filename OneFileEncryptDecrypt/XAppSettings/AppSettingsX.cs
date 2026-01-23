using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OneFileEncryptDecrypt.XAppSettings
{
    public class AppSettingsX
    {
        public string CryptoTempDirectoryPath { get; private set; }
        public XMessage.WorkMessageSet WorkMessage { get; private set; }

        // --------------------------------------------

        private string CreateWorkDirectoryName
        {
            get
            {
                var rndText = Path.GetRandomFileName().Replace(".", string.Empty);
                var dnt = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fffff");
                var result = $"Work__{dnt}__{rndText}";

                return result;
            }
        }

        // --------------------------------------------

        private string CreateXDirectoryPath(string dirPath, string dirName)
        {
            var result = Path.Combine(dirPath, dirName);

            if (Directory.Exists(result) == false)
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        // --------------------------------------------

        public AppSettingsX()
        {
            var callSign = XValue.ProcessValue.ApplicationCallSign;
            var langCode = CultureInfo.CurrentUICulture.Name;
            var workDirPath = Environment.CurrentDirectory;

            this.CryptoTempDirectoryPath = this.CreateXDirectoryPath(workDirPath, $"{callSign}CryptoTemp");
            this.WorkMessage = new XMessage.WorkMessageSet(langCode);
        }

        public XModel.CryptoXFilePath CreateCryptoWorkPath()
        {
            var tempDirPath = this.CryptoTempDirectoryPath;
            var workDirName = this.CreateWorkDirectoryName;
            var workDirPath = this.CreateXDirectoryPath(tempDirPath, workDirName);
            var result = new XModel.CryptoXFilePath(workDirPath);

            return result;
        }
    }
}
