using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class DecryptWork
    {
        public static void ExecuteNow(XModel.WorkOrder wo)
        {
            Console.WriteLine($"DecryptWork... ({wo.CryptoKey}) {wo.FilePath}");
        }
    }
}
