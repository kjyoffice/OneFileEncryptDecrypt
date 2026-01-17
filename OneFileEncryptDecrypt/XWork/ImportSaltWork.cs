using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class ImportSaltWork
    {
        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, string importFilePath)
        {
            CreateSaltWork.BackupSaltFile(asx, cwms);

            File.Copy(importFilePath, asx.Crypto.SaltFilePath, true);

            // 암호화, 복호화 Salt를 가져왔습니다.
            cwms.Success.MessageNow(asx.WorkMessage.ImportSaltDone);
        }
    }
}
