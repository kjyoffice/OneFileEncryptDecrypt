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
            var dnt = DateTime.Now.ToString("yyyyMMdd_HHmmss_fffff");
            var newFileName = $"{fileName}__Backup__{dnt}{fileExt}";
            var result = Path.Combine(dirPath, newFileName);

            return result;
        }

        private static void BackupSaltFile(XAppSettings.AppSettingsX asx)
        {
            if (asx.Crypto.IsExistSaltFile == true)
            {
                var saltFilePath = asx.Crypto.SaltFilePath;
                var bakFilePath = CreateSaltWork.CreateBackupFilePath(saltFilePath);

                File.Copy(saltFilePath, bakFilePath);
            }
        }

        // -----------------------------------------------------------

        public static void ExecuteNow(XAppSettings.AppSettingsX asx)
        {
            CreateSaltWork.BackupSaltFile(asx);

            var saltBT = RandomNumberGenerator.GetBytes(16);

            File.WriteAllBytes(asx.Crypto.SaltFilePath, saltBT);
        }
    }
}
