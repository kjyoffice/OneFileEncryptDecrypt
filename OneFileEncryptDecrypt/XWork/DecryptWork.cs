using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class DecryptWork
    {
        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XModel.CryptoWorkOrder cwo)
        {
            Console.WriteLine($"DecryptWork... ({cwo.CryptoKey}) {cwo.FilePath}");
        }
    }
}
