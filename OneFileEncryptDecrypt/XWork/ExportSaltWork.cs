using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class ExportSaltWork
    {
        private static string CreateExportFilePath(string filePath, string exportDirectoryPath)
        {
            var callSign = XValue.ProcessValue.ApplicationCallSign;
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var fileExt = Path.GetExtension(filePath);
            var dnt = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fffff");
            var newFileName = $"{callSign}_{fileName}__{dnt}{fileExt}";
            var result = Path.Combine(exportDirectoryPath, newFileName);

            return result;
        }

        // -----------------------------------------------------------

        public static void ExecuteNow(XAppSettings.AppSettingsX asx, XConsole.ConsoleWriteMessageSet cwms, string exportDirectoryPath)
        {
            var filePath = asx.Crypto.SaltFilePath;
            var exportFilePath = ExportSaltWork.CreateExportFilePath(filePath, exportDirectoryPath);

            if (File.Exists(exportFilePath) == false)
            {
                File.Copy(filePath, exportFilePath, true);

                // 암호화, 복호화 Salt를 내보냈습니다.
                cwms.Success.MessageNow(asx.WorkMessage.ExportSaltDone);
            }
            else
            {
                // 내보낼 암호화, 복호화 Salt를 저장할 파일이 존재합니다.
                cwms.Error.MessageNow(asx.WorkMessage.AlreadyExistExportSaltFile);
            }
        }
    }
}
