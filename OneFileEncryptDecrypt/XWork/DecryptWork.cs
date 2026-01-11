using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class DecryptWork
    {
        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, XModel.CryptoWorkOrder cwo)
        {
            cwms.Normal.MessageNow($"DecryptWork... ({cwo.CryptoKey}) {cwo.FilePath}");
        }
    }
}
