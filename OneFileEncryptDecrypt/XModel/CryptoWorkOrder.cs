using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoWorkOrder
    {
        public string CryptoKey { get; private set; }
        public string FilePath { get; private set; }

        // ----------------------------------------------------------------

        public CryptoWorkOrder(string cryptoKey, string filePath)
        {
            this.CryptoKey = cryptoKey;
            this.FilePath = filePath;
        }
    }
}
