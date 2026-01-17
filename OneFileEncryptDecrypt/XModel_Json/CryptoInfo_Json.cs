using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel_Json
{
    public class CryptoInfo_Json
    {
        [Newtonsoft.Json.JsonProperty("version")]
        public int CryptoVersion { get; set; }
        
        [Newtonsoft.Json.JsonProperty("mode")]
        public string? CryptoMode { get; set; }
        
        [Newtonsoft.Json.JsonProperty("keyType")]
        public string? CryptoKeyType { get; set; }
        
        [Newtonsoft.Json.JsonProperty("keyIterations")]
        public int CryptoKeyIterations { get; set; }

        [Newtonsoft.Json.JsonProperty("workDateTime")]
        public string? WorkDateTime { get; set; }
    }
}
