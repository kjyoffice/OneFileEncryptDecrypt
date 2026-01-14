using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OneFileEncryptDecrypt.XMessage
{
    public class WorkMessageSet
    {
        private bool IsHangul { get; set; }

        // ------------------------------------------------

        public string EmptyOrWrongAppSettings
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "AppSettings이 없거나 올바르지 않습니다." :
                    "Empty or wrong AppSettings."
                );
            }
        }

        public string AppDescription
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "하나의 파일을 암호화, 복호화 합니다." :
                    "One file encrypt and decrypt work."
                );
            }
        }

        public string CreateSaltDescription
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "암호화, 복호화 Salt를 생성합니다." :
                    $"Create crypto salt."
                );
            }
        }

        public string CreateSaltDone
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "암호화, 복호화 Salt를 생성했습니다." :
                    $"Create done crypto salt."
                );
            }
        }

        public string BackupSaltDone
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "이미 생성된 암호화, 복호화 Salt를 백업했습니다." :
                    "Already have crypto salt is backup."
                );
            }
        }

        public string ReadFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "파일 읽기" :
                    $"Read file"
                );
            }
        }

        public string OriginalChecksum
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "원본파일 Checksum" :
                    "Original file checksum"
                );
            }
        }

        public string EncryptFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "파일 암호화" :
                    "Encrypt file"
                );
            }
        }

        public string EncryptChecksum
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "암호화 파일 Checksum" :
                    "Encrypt file checksum"
                );
            }
        }

        public string SaveEncryptFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "암호화 된 파일 저장" :
                    "Save encrypt file"
                );
            }
        }

        public string ZIPCompressionFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "최종파일 저장" :
                    "Save final file"
                );
            }
        }

        public string EncryptFileDone
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "파일을 암호화 했습니다." :
                    "File encrypt done."
                );
            }
        }

        public List<string> AlreadyExistEncryptFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    new List<string>() { "같은 이름의 암호화된 파일이 있습니다.", "진행이 중단 됐습니다." } :
                    new List<string>() { "Exist same encrypt file", "Stop this process." }
                );
            }
        }

        public string ZIPExtractFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "파일 분리" :
                    "Separate file"
                );
            }
        }

        public string DecryptFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "파일 복호화" :
                    "Decrypt file"
                );
            }
        }

        public string DecryptChecksum
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "복호화 파일 Checksum" :
                    "Decrypt file checksum"
                );
            }
        }

        public string SaveDecryptFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "복호화 된 파일 저장" :
                    "Save decrypt file"
                );
            }
        }

        public string DecryptFileDone
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "파일을 복호화 했습니다." :
                    "File decrypt done."
                );
            }
        }

        public string DifferentEncryptChecksum
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "암호화 파일 해쉬가 다릅니다." :
                    "Different encrypt checksum."
                );
            }
        }

        public string DifferentDecryptChecksum
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "복호화 파일 해쉬가 다릅니다." :
                    "Different decrypt checksum."
                );
            }
        }

        public List<string> AlreadyExistDecryptFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    new List<string>() { "같은 이름의 복호화된 파일이 있습니다.", "진행이 중단 됐습니다." } :
                    new List<string>() { "Exist same decrypt file", "Stop this process." }
                );
            }
        }

        public List<string> EncryptPasswordMemoryNotify
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    new List<string>() { "암호화 비밀번호는 아주 중요합니다.", "잊지 않도록 해주세요!" } :
                    new List<string>() { "Encrypt password is very important", "Don't forget!" }
                );
            }
        }

        public string DecryptFail
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "복호화에 실패했습니다." :
                    "Decrypt fail."
                );
            }
        }

        public List<string> NotEmptyEncryptDirectory
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    new List<string>() { "암호화 작업 디렉토리가 비어있지 않습니다.", "진행이 중단 됐습니다." } :
                    new List<string>() {"Not empty encrypt directory", "Stop this process." }
                );
            }
        }

        public string NotExistDecryptRequireFile
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "존재하지 않는 복호화 필수 파일이 있습니다." :
                    "Not exist decrypt require file."
                );
            }
        }

        /*
        public string AppDescription
        {
            get
            {
                return (
                    (this.IsHangul == true) ?
                    "" :
                    ""
                );
            }
        }
        */

        // ------------------------------------------------

        private string CryptoCommandText(bool isHangul, string commandName)
        {
            var encCmdName = XCommand.CryptoCommand.EncryptCommandName;
            var decCmdName = XCommand.CryptoCommand.DecryptCommandName;
            var result = string.Empty;

            if (commandName == encCmdName)
            {
                result = ((isHangul == true) ? "암호화" : "Encrypt");
            }
            else if (commandName == decCmdName)
            {
                result = ((isHangul == true) ? "복호화" : "Decrypt");
            }

            return result;
        }

        // ------------------------------------------------

        public WorkMessageSet(string languageCode)
        {
            this.IsHangul = (languageCode.ToUpper() == "KO-KR");
        }

        public List<string> NotExistCryptoSalt(string commandName)
        {
            return (
                (this.IsHangul == true) ?
                new List<string>() { $"암호화, 복호화 Salt가 없습니다.", $"다음의 명령을 실행해주세요. {commandName}." } :
                new List<string>() { $"Not exist crypto salt.", $"Please run {commandName}." }
            );
        }

        public string CryptoCommandDescription(string commandName)
        {
            var isHangul = this.IsHangul;
            var cmdText = this.CryptoCommandText(isHangul, commandName);
            var result = (
                (isHangul == true) ?
                $"파일을 {cmdText} 합니다." :
                $"{cmdText} a file"
            );

            return result;
        }

        public string CryptoPasswordDescription(string commandName)
        {
            var isHangul = this.IsHangul;
            var cmdText = this.CryptoCommandText(isHangul, commandName);
            var result = (
                (isHangul == true) ?
                $"{cmdText} 비밀번호" :
                $"{cmdText} password"
            );

            return result;
        }

        public string CryptoPasswordNotAllowLength(string tkText, int keyMinLength)
        {
            var isHangul = this.IsHangul;
            var result = (
                (isHangul == true) ?
                $"{tkText}비밀번호는 최소 {keyMinLength}자 이상이어야 합니다." :
                $"{tkText}Want password length minimum {keyMinLength}."
            );

            return result;
        }

        public string CryptoFileDescription(string commandName)
        {
            var isHangul = this.IsHangul;
            var cmdText = this.CryptoCommandText(isHangul, commandName);
            var result = (
                (isHangul == true) ?
                $"{cmdText} 파일 경로" :
                $"{cmdText} file path"
            );

            return result;
        }

        public string CryptoFileNotExist(string tkText)
        {
            var isHangul = this.IsHangul;
            var result = (
                (isHangul == true) ?
                $"{tkText}파일이 존재하지 않습니다." :
                $"{tkText}Not exist file."
            );

            return result;
        }

        public string CryptoFileBigNotSupport(string tkText, int maxSizeMB)
        {
            var isHangul = this.IsHangul;
            var result = (
                (isHangul == true) ?
                $"{tkText}{maxSizeMB} MB 이상의 파일은 지원하지 않습니다." :
                $"{tkText}Not support {maxSizeMB} MB over file."
            );

            return result;
        }

        public string DecryptFileWrong(string tkText, string commandName)
        {
            var isHangul = this.IsHangul;
            var cmdText = this.CryptoCommandText(isHangul, commandName);
            var result = (
                (isHangul == true) ?
                $"{tkText}{cmdText} 파일이 올바르지 않습니다." :
                $"{tkText}Wrong {cmdText.ToLower()} file"
            );

            return result;
        }

        /*
        public string CryptoCommandDescriptionX(string commandName)
        {
            var isHangul = this.IsHangul;
            var cmdText = this.CryptoCommandText(isHangul, commandName);
            var result = (
                (isHangul == true) ?
                $"" :
                $""
            );

            return result;
        }

        public string NotExistCryptoSalt(string commandName)
        {
            return (
                (this.IsHangul == true) ?
                $"" :
                $""
            );
        }
        */
    }
}
