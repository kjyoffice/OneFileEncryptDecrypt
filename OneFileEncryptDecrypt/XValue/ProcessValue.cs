using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OneFileEncryptDecrypt.XValue
{
    public class ProcessValue
    {
        public static int CryptoPasswordMinimumLength
        {
            get
            {
                return 10;
            }
        }

        public static int FileAllowMaxSizeMB
        {
            get
            {
                return 50;
            }
        }

        public static int BufferChunkSize
        {
            get
            {
                return (1_048_576 * 4); // 4MB;
            }
        }

        public static string CryptoMode_AES256CBC
        {
            get
            {
                return "AES256CBC";
            }
        }

        public static string CryptoMode_AES256GCM
        {
            get
            {
                return "AES256GCM";
            }
        }

        public static string ApplicationPublicTitle
        {
            get
            {
                return "OneFileEncryptDecrypt";
            }
        }

        public static string ApplicationCallSign
        {
            get
            {
                return "OFED";
            }
        }

        public static string WorkFileExtension
        {
            get
            {
                return ".ofed";
            }
        }

        public static string WorkFileExtension_DoneX
        {
            get
            {
                return ".ofedx";
            }
        }

        public static int CryptoVersion1
        {
            get
            {
                return 1;
            }
        }

        public static int TagSizeBits
        {
            get
            {
                // 16 bytes
                return 128;
            }
        }

        public static string CryptoBackup_TRUE
        {
            get
            {
                return "TRUE";
            }
        }

        public static string CryptoBackup_FALSE
        {
            get
            {
                return "FALSE";
            }
        }

        // ---------------------------------------------------------------

        public static string CryptoModeDisplay(string cryptoMode)
        {
            var dic = new Dictionary<string, string>()
            {
                { ProcessValue.CryptoMode_AES256CBC, "AES256 CBC" },
                { ProcessValue.CryptoMode_AES256GCM, "AES256 GCM" },
            };
            var result = ((dic.ContainsKey(cryptoMode) == true) ? dic[cryptoMode] : "Unknown");

            return result;
        }
    }
}
