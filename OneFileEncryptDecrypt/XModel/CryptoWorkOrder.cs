using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoWorkOrder
    {
        public byte[] CryptoPassword { get; private set; }
        public string FilePath { get; private set; }

        // ----------------------------------------------------------------

        public CryptoWorkOrder(string cryptoPassword, string filePath)
        {
            this.CryptoPassword = Encoding.UTF8.GetBytes(cryptoPassword);
            this.FilePath = filePath;
        }
    }
}
