using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class EncryptWork
    {
        public static void ExecuteNow(XModel.WorkOrder wo)
        {
            Console.WriteLine($"EncryptWork... ({wo.CryptoKey}) {wo.FilePath}");
        }
    }
}
