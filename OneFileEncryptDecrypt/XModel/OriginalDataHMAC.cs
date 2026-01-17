using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class OriginalDataHMAC
    {
        public byte[] OriginalData { get; private set; }
        public byte[] OriginalHMAC { get; private set; }

        // -----------------------------------------------------------------

        public OriginalDataHMAC(byte[] originalData, byte[] originalHMAC)
        {
            this.OriginalData = originalData;
            this.OriginalHMAC = originalHMAC;
        }
    }
}
