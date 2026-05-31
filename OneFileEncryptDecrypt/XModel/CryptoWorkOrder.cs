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
        public bool IsCryptoBackup { get; private set; }

        // ----------------------------------------------------------------

        private string CryptoModeConfirm(string cryptoMode, bool isEncrypt)
        {
            var defaultValue = XValue.ProcessValue.CryptoMode_AES256CBC;
            var useValue = ((cryptoMode == string.Empty) ? defaultValue : cryptoMode);
            var result = ((isEncrypt == true) ? useValue : string.Empty).ToUpper();

            return result;
        }

        private bool IsCryptoBackupConfirm(string cryptoBackup, bool isEncrypt)
        {
            var cryptoBackup_TRUE = XValue.ProcessValue.CryptoBackup_TRUE;
            var cryptoBackup_FALSE = XValue.ProcessValue.CryptoBackup_FALSE;
            var useValue = ((cryptoBackup == string.Empty) ? cryptoBackup_TRUE : cryptoBackup);
            var confirmValue = ((isEncrypt == true) ? useValue : cryptoBackup_FALSE);
            var result = Convert.ToBoolean(confirmValue);

            return result;
        }

        // ----------------------------------------------------------------

        public CryptoWorkOrder(string cryptoPassword, string sourceFilePath, string cryptoMode, string cryptoBackup, bool isEncrypt)
        {
            this.CryptoPassword = Encoding.UTF8.GetBytes(cryptoPassword);
            this.SourceFilePath = sourceFilePath;
            this.CryptoMode = this.CryptoModeConfirm(cryptoMode, isEncrypt);
            this.IsCryptoBackup = this.IsCryptoBackupConfirm(cryptoBackup, isEncrypt);
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

        public string CreateSourceFileBackupPath()
        {
            var filePath = this.SourceFilePath;
            var dirPath = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var dnt = DateTime.Now.ToString("yyyyMMdd_HHmmss_fffff");
            var rndFN = Path.GetRandomFileName().Replace(".", string.Empty);
            var fileExt = Path.GetExtension(filePath);
            var newFileName = $"{fileName}___Backup__{dnt}__{rndFN}{fileExt}";
            var result = Path.Combine(dirPath!, newFileName);

            return result;
        }
    }
}
