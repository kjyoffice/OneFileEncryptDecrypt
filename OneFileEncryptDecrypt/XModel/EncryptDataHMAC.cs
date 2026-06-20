using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class EncryptDataHMAC
    {
        public byte[] CryptoIV { get; private set; }
        public byte[] EncryptData { get; private set; }
        public byte[] EncryptHMAC { get; private set; }
        public int EncryptVersion { get; private set; }

        // -----------------------------------------------------------------

        public EncryptDataHMAC(byte[] cryptoIV, byte[] encryptData, byte[] encryptHMAC, int encryptVersion)
        {
            this.CryptoIV = cryptoIV;
            this.EncryptData = encryptData;
            this.EncryptHMAC = encryptHMAC;
            this.EncryptVersion = encryptVersion;
        }

        public EncryptDataHMAC(byte[] cryptoIV, byte[] encryptData, byte[] encryptHMAC) : this(cryptoIV, encryptData, encryptHMAC, 0)
        {
            // Empty
        }
    }
}
