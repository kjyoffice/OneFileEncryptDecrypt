using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class EncryptDataAndVersion
    {
        public List<byte> EncryptData { get; private set; }
        public int EncryptVersion { get; private set; }

        // -----------------------------------------------------------------

        public EncryptDataAndVersion()
        {
            this.EncryptData = new List<byte>();
            this.EncryptVersion = 0;
        }

        public void ChangeData(byte[] encData, int encVersion)
        {
            this.EncryptData.AddRange(encData);
            this.EncryptVersion = encVersion;
        }
    }
}
