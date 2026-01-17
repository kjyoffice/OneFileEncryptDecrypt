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

        private string CryptoModeConfirm(string cryptoMode, bool isEncrypt)
        {
            var defaultCryptoMode = XValue.ProcessValue.CryptoMode_AES256CBC;
            var cryptoModeUse = ((cryptoMode == string.Empty) ? defaultCryptoMode : cryptoMode);
            var result = ((isEncrypt == true) ? cryptoModeUse : string.Empty).ToUpper();

            return result;
        }

        // ----------------------------------------------------------------

        public CryptoWorkOrder(string cryptoPassword, string sourceFilePath, string cryptoMode, bool isEncrypt)
        {
            this.CryptoPassword = Encoding.UTF8.GetBytes(cryptoPassword);
            this.SourceFilePath = sourceFilePath;
            this.CryptoMode = this.CryptoModeConfirm(cryptoMode, isEncrypt);
        }

        public string CreateEncryptZIPFilePath()
        {
            var workDoneFileExt = XValue.ProcessValue.WorkFileExtension_DoneX;
            var sourceFilePath = this.SourceFilePath;
            var dirPath = Path.GetDirectoryName(sourceFilePath)!;
            var newFileName = (Path.GetFileName(sourceFilePath) + workDoneFileExt);
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
