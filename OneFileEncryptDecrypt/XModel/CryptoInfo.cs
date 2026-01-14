using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoInfo
    {
        [Newtonsoft.Json.JsonProperty("cryptoVersion")]
        public int CryptoVersion { get; private set; }
        [Newtonsoft.Json.JsonProperty("cryptoDateTime")]
        public string CryptoDateTime { get; private set; }

        // --------------------------------------------

        public CryptoInfo(int cryptoVersion)
        {
            this.CryptoVersion = cryptoVersion;
            this.CryptoDateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffff");
        }
    }
}
