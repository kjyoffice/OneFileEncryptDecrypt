using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XValue
{
    public class ProcessValue
    {
        public static int CryptoKeyMinimumLength
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
                return 30;
            }
        }

        public static int BufferChunkSize
        {
            get
            {
                return (1_048_576 * 4); // 4MB;
            }
        }
    }
}
