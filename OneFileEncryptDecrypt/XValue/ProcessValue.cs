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

        public static long FileAllowMaxSizeMB
        {
            get
            {
                return 300L;
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
