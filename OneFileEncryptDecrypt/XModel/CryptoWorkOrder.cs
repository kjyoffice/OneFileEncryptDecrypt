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
        public string EncryptTimeZIPFilePath { get; private set; }
        public string DecryptTimeOriginalFIlePath { get; private set; }

        // ----------------------------------------------------------------

        private string CreateEncryptTimeZIPFilePath(string commandName, string sourceFilePath)
        {
            var encCmdName = XCommand.CryptoCommand.EncryptCommandName;
            var dirPath = Path.GetDirectoryName(sourceFilePath)!;
            var newFileName = (Path.GetFileName(sourceFilePath) + ".ofedx");
            var result = ((commandName == encCmdName) ? Path.Combine(dirPath, newFileName) : string.Empty);

            return result;
        }

        private string CreateDecryptTimeOriginalFIlePath(string commandName, string sourceFilePath)
        {
            var decCmdName = XCommand.CryptoCommand.DecryptCommandName;
            var dirPath = Path.GetDirectoryName(sourceFilePath)!;
            var fileName = Path.GetFileName(sourceFilePath);
            var fileExt = Path.GetExtension(fileName);
            var sliceEnd = (fileName.Length - fileExt.Length);
            var newFileName = fileName.Substring(0, sliceEnd);
            var result = ((commandName == decCmdName) ? Path.Combine(dirPath, newFileName) : string.Empty);

            return result;
        }

        // ----------------------------------------------------------------

        public CryptoWorkOrder(string commandName, string cryptoPassword, string sourceFilePath)
        {
            this.CryptoPassword = Encoding.UTF8.GetBytes(cryptoPassword);
            this.SourceFilePath = sourceFilePath;
            this.EncryptTimeZIPFilePath = this.CreateEncryptTimeZIPFilePath(commandName, sourceFilePath);
            this.DecryptTimeOriginalFIlePath = this.CreateDecryptTimeOriginalFIlePath(commandName, sourceFilePath);
        }
    }
}
