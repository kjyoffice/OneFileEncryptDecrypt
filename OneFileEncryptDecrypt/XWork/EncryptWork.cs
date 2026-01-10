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
            //var text = "HelloWorld~";
            //var textBT = Encoding.UTF8.GetBytes("HelloWorld~");
            //var salt = "world";
            //var key = XCrypto.AES256ProcessX.CreateKey("hello", salt);
            //var iv = XCrypto.AES256ProcessX.CreateIV();
            //var encData = XCrypto.AES256X.EncryptNow(key, iv, textBT);
            //var decData = XCrypto.AES256X.DecryptNow(key, iv, encData);
            //var decText = Encoding.UTF8.GetString(decData);
            //Console.WriteLine(text);
            //Console.WriteLine("------------------------------------------------------------");
            //Console.WriteLine(XCrypto.CryptoWork.CreateHash(textBT));
            //Console.WriteLine("------------------------------------------------------------");
            //Console.WriteLine(BitConverter.ToString(encData));
            //Console.WriteLine("------------------------------------------------------------");
            //Console.WriteLine(BitConverter.ToString(decData));
            //Console.WriteLine(decText);
            //Console.WriteLine("------------------------------------------------------------");
            //Console.WriteLine(XCrypto.CryptoWork.CreateHash(decData));
            //Console.WriteLine("------------------------------------------------------------");
            //Console.WriteLine(text == decText);
            //Console.WriteLine(XCrypto.CryptoWork.CreateHash(textBT) == XCrypto.CryptoWork.CreateHash(decData));

            //var pv = new XModel.ProgressViewer();
            //var salt = "world";
            //var key = XCrypto.AES256ProcessX.CreateKey("hello", salt);
            //var iv = XCrypto.AES256ProcessX.CreateIV();
            //var fileBT = FileWork.GetFileByte(wo.FilePath, "Read file...", pv);
            //var hash1 = XCrypto.HashWork.CreateSHA512(fileBT, "File hash...", pv);
            //var enc = XCrypto.AES256X.EncryptNow(key, iv, fileBT, "Encrypt...", pv);
            //FileWork.WriteFileByte(enc, @"D:\Download\Dummy\Hello_End.txt", "Save encrypt file...", pv);
            //var dec = XCrypto.AES256X.DecryptNow(key, iv, enc, "Decrypt...", pv);
            //var hash2 = XCrypto.HashWork.CreateSHA512(dec, "Decrypt after hash...", pv);
            //FileWork.WriteFileByte(dec, @"D:\Download\Dummy\Hello_EndDec.txt", "Save decrypt file...", pv);

            //Console.WriteLine(hash1);
            //Console.WriteLine(hash2);
            //Console.WriteLine(hash1 == hash2);

            Console.WriteLine($"EncryptWork... ({wo.CryptoKey}) {wo.FilePath}");
        }
    }
}
