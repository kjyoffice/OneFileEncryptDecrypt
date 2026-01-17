using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoWorkOrder
    {
        public byte[] CryptoPassword { get; private set; }
        public string SourceFilePath { get; private set; }
        public string CryptoMode { get; private set; }

        // ----------------------------------------------------------------

        public CryptoWorkOrder(string cryptoPassword, string sourceFilePath)
        {
            this.CryptoPassword = Encoding.UTF8.GetBytes(cryptoPassword);
            this.SourceFilePath = sourceFilePath;
            this.CryptoMode = XValue.ProcessValue.CryptoMode_AESCBC;
        }

        public string CreateEncryptZIPFilePath()
        {
            var sourceFilePath = this.SourceFilePath;
            var dirPath = Path.GetDirectoryName(sourceFilePath)!;
            var newFileName = (Path.GetFileName(sourceFilePath) + ".ofedx");
            var result = Path.Combine(dirPath, newFileName);

            return result;
        }

        public string CreateDecryptOriginalFIlePath()
        {
            var sourceFilePath = this.SourceFilePath;
            var dirPath = Path.GetDirectoryName(sourceFilePath)!;
            var fileName = Path.GetFileName(sourceFilePath);
            var fileExt = Path.GetExtension(fileName);
            var sliceEnd = (fileName.Length - fileExt.Length);
            var newFileName = fileName.Substring(0, sliceEnd);
            var result = Path.Combine(dirPath, newFileName);

            return result;
        }
    }
}
