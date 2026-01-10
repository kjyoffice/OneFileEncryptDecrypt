using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace OneFileEncryptDecrypt.XWork
{
    public class EncryptWork
    {
        public static void ExecuteNow(XModel.WorkOrder wo)
        {
            var text = "HelloWorld~";
            var textBT = Encoding.UTF8.GetBytes("HelloWorld~");
            var salt = "world";
            var key = XCrypto.CryptoWork.CreateAES256Key("hello", salt);
            var iv = XCrypto.CryptoWork.CreateAES256IV();
            var encData = XCrypto.CryptoWork.AES256Encrypt(key, iv, textBT);
            var decData = XCrypto.CryptoWork.AES256Decrypt(key, iv, encData);
            var decText = Encoding.UTF8.GetString(decData);
            Console.WriteLine(text);
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine(BitConverter.ToString(encData));
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine(BitConverter.ToString(decData));
            Console.WriteLine(decText);
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine(text == decText);

            Console.WriteLine($"EncryptWork... ({wo.CryptoKey}) {wo.FilePath}");
        }
    }
}
