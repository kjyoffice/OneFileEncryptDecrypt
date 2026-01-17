using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoInfo
    {
        [Newtonsoft.Json.JsonIgnore]
        public bool IsAllow { get; private set; }
        [Newtonsoft.Json.JsonProperty("cryptoMode")]
        public string CryptoMode { get; private set; }
        [Newtonsoft.Json.JsonProperty("cryptoVersion")]
        public int CryptoVersion { get; private set; }
        [Newtonsoft.Json.JsonProperty("cryptoDateTime")]
        public string CryptoDateTime { get; private set; }

        // --------------------------------------------

        private string ConvertDateText(DateTime dnt)
        {
            return dnt.ToString("yyyy-MM-dd HH:mm:ss.fffff");
        }

        private DateTime ConvertDnT(XModel_Json.CryptoInfo_Json? jsonData)
        {
            var cryptoDateTime = (jsonData?.CryptoDateTime ?? string.Empty).ToString();
            var result = (((cryptoDateTime != string.Empty) && (DateTime.TryParse(cryptoDateTime, out DateTime dnt) == true)) ? dnt : DateTime.MinValue);

            return result;
        }

        // --------------------------------------------

        public CryptoInfo(string cryptoMode, int cryptoVersion)
        {
            this.IsAllow = true;
            this.CryptoMode = cryptoMode;
            this.CryptoVersion = cryptoVersion;
            this.CryptoDateTime = this.ConvertDateText(DateTime.UtcNow);
        }

        public CryptoInfo(XModel_Json.CryptoInfo_Json? jsonData)
        {
            var cryptoMode = (jsonData?.CryptoMode ?? string.Empty).ToString().ToUpper();
            var cryptoVersion = (jsonData?.CryptoVersion ?? 0);
            var cryptoDateTime = this.ConvertDnT(jsonData);

            this.IsAllow = ((cryptoMode != string.Empty) && (cryptoVersion > 0) && (cryptoDateTime != DateTime.MinValue));
            this.CryptoMode = cryptoMode;
            this.CryptoVersion = cryptoVersion;
            this.CryptoDateTime = this.ConvertDateText(cryptoDateTime);
        }
    }
}
