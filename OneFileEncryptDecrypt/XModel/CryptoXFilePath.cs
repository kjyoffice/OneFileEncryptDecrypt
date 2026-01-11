using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoXFilePath
    {
        public string WorkDirectoryPath { get; private set; }
        public string EncryptDataFilePath { get; private set; }
        public string EncryptDataChecksumFilePath { get; private set; }
        public string OriginalChecksumFilePath { get; private set; }
        public string CryptoIVFilePath { get; private set; }

        // ----------------------------------------------------------

        public CryptoXFilePath(string workDirPath)
        {
            this.WorkDirectoryPath = workDirPath;
            this.EncryptDataFilePath = Path.Combine(workDirPath, "EncryptData.ofed");
            this.EncryptDataChecksumFilePath = Path.Combine(workDirPath, "EncryptDataChecksum.ofed");
            this.OriginalChecksumFilePath = Path.Combine(workDirPath, "OriginalChecksum.ofed");            
            this.CryptoIVFilePath = Path.Combine(workDirPath, "CryptoIV.ofed");
        }
    }
}
