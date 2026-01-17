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

        public static string CryptoMode_AESCBC
        {
            get
            {
                return "AESCBC";
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
    }
}
