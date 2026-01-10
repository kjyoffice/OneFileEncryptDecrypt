using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace OneFileEncryptDecrypt.XCrypto
{
    public class CryptoWork
    {
        public static string CreateHash(byte[] source)
        {
            var result = string.Empty;

            using (var hash = SHA512.Create())
            {
                var hashBT = hash.ComputeHash(source);
                var hashTet = BitConverter.ToString(hashBT);

                result = Regex.Replace(hashTet, "[^0-9A-Za-z]", string.Empty, RegexOptions.IgnoreCase).Trim().ToLower();
                
                hash.Clear();
            }

            return result;
        }
    }
}
