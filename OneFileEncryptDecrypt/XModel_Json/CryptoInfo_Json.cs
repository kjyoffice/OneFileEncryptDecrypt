using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel_Json
{
    public class CryptoInfo_Json
    {
        [Newtonsoft.Json.JsonProperty("cryptoMode")]
        public string? CryptoMode { get; set; }
        [Newtonsoft.Json.JsonProperty("cryptoVersion")]
        public int CryptoVersion { get; set; }
        [Newtonsoft.Json.JsonProperty("cryptoDateTime")]
        public string? CryptoDateTime { get; set; }
    }
}
