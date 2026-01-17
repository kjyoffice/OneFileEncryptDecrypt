using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class CreateSaltWork
    {
        private static string CreateBackupFilePath(string filePath)
        {
            var dirPath = Path.GetDirectoryName(filePath)!;
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var fileExt = Path.GetExtension(filePath);
            var dnt = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fffff");
            var newFileName = $"{fileName}__Backup__{dnt}{fileExt}";
            var result = Path.Combine(dirPath, newFileName);

            return result;
        }

        public static void BackupSaltFile(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            if (asx.Crypto.IsExistSaltFile == true)
            {
                var saltFilePath = asx.Crypto.SaltFilePath;
                var bakFilePath = CreateSaltWork.CreateBackupFilePath(saltFilePath);

                File.Copy(saltFilePath, bakFilePath);

                // 이미 생성된 암호화, 복호화 Salt를 백업했습니다.
                cwms.Warning.MessageNow(asx.WorkMessage.BackupSaltDone, true);
            }
        }

        // -----------------------------------------------------------

        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms)
        {
            CreateSaltWork.BackupSaltFile(asx, cwms);

            var saltBT = RandomNumberGenerator.GetBytes(16);

            File.WriteAllBytes(asx.Crypto.SaltFilePath, saltBT);

            // 암호화, 복호화 Salt를 생성했습니다.
            cwms.Success.MessageNow(asx.WorkMessage.CreateSaltDone);
        }
    }
}
