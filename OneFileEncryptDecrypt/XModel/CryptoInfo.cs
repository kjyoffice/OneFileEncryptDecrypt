using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoInfo
    {
        [Newtonsoft.Json.JsonProperty("cryptoMode")]
        public string CryptoMode { get; private set; }
        [Newtonsoft.Json.JsonProperty("cryptoVersion")]
        public int CryptoVersion { get; private set; }
        [Newtonsoft.Json.JsonProperty("cryptoDateTime")]
        public string CryptoDateTime { get; private set; }

        // --------------------------------------------

        public CryptoInfo(string cryptoMode, int cryptoVersion)
        {
            this.CryptoMode = cryptoMode;
            this.CryptoVersion = cryptoVersion;
            this.CryptoDateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffff");
        }
    }
}
