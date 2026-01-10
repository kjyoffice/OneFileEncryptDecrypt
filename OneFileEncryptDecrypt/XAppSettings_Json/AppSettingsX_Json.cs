using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XAppSettings_Json
{
    public class AppSettingsX_Json
    {
        //[Microsoft.Extensions.Configuration.ConfigurationKeyName("crypto")]
        public AppSettingsX_Crypto_Json? Crypto { get; set; }
    }
}
