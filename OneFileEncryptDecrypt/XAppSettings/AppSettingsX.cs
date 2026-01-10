using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XAppSettings
{
    public class AppSettingsX
    {
        public bool IsAllow { get; private set; }
        public AppSettingsX_Crypto Crypto { get; private set; }

        // --------------------------------------------

        public AppSettingsX(XAppSettings_Json.AppSettingsX_Json? jsonData)
        {
            var crypto = new AppSettingsX_Crypto(jsonData?.Crypto);

            this.IsAllow = (crypto.IsAllow == true);
            this.Crypto = crypto;
        }
    }
}
