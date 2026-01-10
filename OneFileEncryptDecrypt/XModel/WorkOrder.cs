using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class WorkOrder
    {
        public string CryptoKey { get; private set; }
        public string FilePath { get; private set; }

        // ----------------------------------------------------------------

        public WorkOrder(string cryptoKey, string filePath)
        {
            this.CryptoKey = cryptoKey;
            this.FilePath = filePath;
        }
    }
}
