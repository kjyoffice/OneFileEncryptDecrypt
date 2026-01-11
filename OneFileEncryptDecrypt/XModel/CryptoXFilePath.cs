using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoXFilePath
    {
        public string WorkDirectoryPath { get; private set; }
        public string EncryptDataFilePath { get; private set; }
        public string EncryptDataChecksumFilePath { get; private set; }
        public string OriginalChecksumFilePath { get; private set; }
        public string CryptoIVFilePath { get; private set; }

        // ----------------------------------------------------------

        public CryptoXFilePath(string workDirPath)
        {
            this.WorkDirectoryPath = workDirPath;
            this.EncryptDataFilePath = Path.Combine(workDirPath, "EncryptData.ofed");
            this.EncryptDataChecksumFilePath = Path.Combine(workDirPath, "EncryptDataChecksum.ofed");
            this.OriginalChecksumFilePath = Path.Combine(workDirPath, "OriginalChecksum.ofed");            
            this.CryptoIVFilePath = Path.Combine(workDirPath, "CryptoIV.ofed");
        }

        public void DeleteAllFile(string sourceFilePath)
        {
            var workDirPath = this.WorkDirectoryPath;
            // 삭제해야 할 파일 리스트
            var filePathList = new List<string>()
            {
                this.EncryptDataFilePath,
                this.EncryptDataChecksumFilePath,
                this.OriginalChecksumFilePath,
                this.CryptoIVFilePath,
                sourceFilePath
            };

            // 파일 삭제 고고
            filePathList.ForEach(x => File.Delete(x));

            // 작업 디렉토리에 파일이 남았는지 체크
            var isExistFile = (Directory.GetFiles(workDirPath, "*.*", SearchOption.AllDirectories).Length > 0);

            // 남아있는 파일이 없으면 작업 디렉토리도 삭제하자
            if (isExistFile == false)
            {
                Directory.Delete(workDirPath);
            }
        }
    }
}
