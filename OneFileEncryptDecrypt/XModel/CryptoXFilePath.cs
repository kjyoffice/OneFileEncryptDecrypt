using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class CryptoXFilePath
    {
        public string WorkDirectoryPath { get; private set; }
        public string EncryptDataFilePath { get; private set; }
        public string EncryptHMACFilePath { get; private set; }
        public string OriginalHMACFilePath { get; private set; }
        public string CryptoIVFilePath { get; private set; }
        public string CryptoInfoFilePath { get; private set; }
        public string CryptoSaltFilePath { get; private set; }

        // ----------------------------------------------------------

        // 암호화때는 해당 폴더가 비어있어야 한다 
        public bool IsEmptyDirectory
        {
            get
            {
                var workDirPath = this.WorkDirectoryPath;
                var isNotExistDir = (Directory.GetDirectories(workDirPath, "*.*", SearchOption.AllDirectories).Length <= 0);
                var isNotExistFile = (Directory.GetFiles(workDirPath, "*.*", SearchOption.AllDirectories).Length <= 0);
                var result = ((isNotExistDir == true) && (isNotExistFile == true));

                return result;
            }
        }

        // 복호화때는 파일이 모두 있어야 한다
        public bool IsAllExistDecryptFile
        {
            get
            {
                return (
                    (File.Exists(this.EncryptDataFilePath) == true) &&
                    (File.Exists(this.EncryptHMACFilePath) == true) &&
                    (File.Exists(this.OriginalHMACFilePath) == true) &&
                    (File.Exists(this.CryptoIVFilePath) == true) &&
                    (File.Exists(this.CryptoInfoFilePath) == true) &&
                    (File.Exists(this.CryptoSaltFilePath) == true)                    
                );
            } 
        }

        // ----------------------------------------------------------

        public CryptoXFilePath(string workDirPath)
        {
            var workFileExt = XValue.ProcessValue.WorkFileExtension;

            this.WorkDirectoryPath = workDirPath;
            this.EncryptDataFilePath = Path.Combine(workDirPath, $"EncryptData{workFileExt}");
            this.EncryptHMACFilePath = Path.Combine(workDirPath, $"EncryptDataHMAC{workFileExt}");
            this.OriginalHMACFilePath = Path.Combine(workDirPath, $"OriginalHMAC{workFileExt}");            
            this.CryptoIVFilePath = Path.Combine(workDirPath, $"CryptoIV{workFileExt}");
            this.CryptoInfoFilePath = Path.Combine(workDirPath, "CryptoInfo.json");
            this.CryptoSaltFilePath = Path.Combine(workDirPath, $"CryptoSalt{workFileExt}");
        }

        public void DeleteAllFile(string sourceFilePath)
        {
            var workDirPath = this.WorkDirectoryPath;
            // 삭제해야 할 파일 리스트
            var filePathList = new List<string>()
            {
                this.EncryptDataFilePath,
                this.EncryptHMACFilePath,
                this.OriginalHMACFilePath,
                this.CryptoIVFilePath,
                this.CryptoInfoFilePath,
                this.CryptoSaltFilePath
            };

            if (sourceFilePath != string.Empty)
            {
                filePathList.Add(sourceFilePath);
            }

            // 파일 삭제 고고
            foreach (var filePath in filePathList)
            {
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }
            }

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
