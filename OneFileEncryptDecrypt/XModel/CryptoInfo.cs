using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoInfo
    {
        [Newtonsoft.Json.JsonIgnore]
        public bool IsAllow { get; private set; }
        [Newtonsoft.Json.JsonProperty("keyType")]
        public string CryptoKeyType { get; private set; }
        [Newtonsoft.Json.JsonProperty("keyIterations")]
        public int CryptoKeyIterations { get; private set; }
        [Newtonsoft.Json.JsonProperty("mode")]
        public string CryptoMode { get; private set; }
        [Newtonsoft.Json.JsonProperty("version")]
        public int CryptoVersion { get; private set; }
        [Newtonsoft.Json.JsonProperty("workDateTime")]
        public string WorkDateTime { get; private set; }

        // --------------------------------------------

        private string ConvertDateText(DateTime dnt)
        {
            return dnt.ToString("yyyy-MM-dd HH:mm:ss.fffff");
        }

        private DateTime ConvertWorkDate(XModel_Json.CryptoInfo_Json? jsonData)
        {
            var workDateTime = (jsonData?.WorkDateTime ?? string.Empty).ToString();
            var result = (((workDateTime != string.Empty) && (DateTime.TryParse(workDateTime, out DateTime dnt) == true)) ? dnt : DateTime.MinValue);

            return result;
        }

        // --------------------------------------------

        public CryptoInfo(string cryptoKeyType, int cryptoKeyIterations, string cryptoMode, int cryptoVersion)
        {
            this.IsAllow = true;
            this.CryptoKeyType = cryptoKeyType;
            this.CryptoKeyIterations = cryptoKeyIterations;
            this.CryptoMode = cryptoMode;
            this.CryptoVersion = cryptoVersion;
            this.WorkDateTime = this.ConvertDateText(DateTime.UtcNow);
        }

        public CryptoInfo(XModel_Json.CryptoInfo_Json? jsonData)
        {
            var cryptoKeyType = (jsonData?.CryptoKeyType ?? string.Empty);
            var cryptoKeyIterations = (jsonData?.CryptoKeyIterations ?? 0);
            var cryptoMode = (jsonData?.CryptoMode ?? string.Empty).ToString().ToUpper();
            var cryptoVersion = (jsonData?.CryptoVersion ?? 0);
            var workDateTime = this.ConvertWorkDate(jsonData);

            this.IsAllow = ((cryptoMode != string.Empty) && (cryptoVersion > 0) && (workDateTime != DateTime.MinValue));
            this.CryptoKeyType = cryptoKeyType;
            this.CryptoKeyIterations = cryptoKeyIterations;
            this.CryptoMode = cryptoMode;
            this.CryptoVersion = cryptoVersion;
            this.WorkDateTime = this.ConvertDateText(workDateTime);
        }
    }
}
