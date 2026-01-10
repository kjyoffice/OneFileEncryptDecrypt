using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace OneFileEncryptDecrypt.XCrypto
{
    public class AES256ProcessX
    {
        public static byte[] CreateKey(string password, string salt)
        {
            var passworBT = Encoding.UTF8.GetBytes(password);
            var saltBT = Encoding.UTF8.GetBytes(salt);
            var iterations = 100_000;

            // SHA-256 기반 PBKDF2
            var digest = new Org.BouncyCastle.Crypto.Digests.Sha256Digest();
            var generator = new Pkcs5S2ParametersGenerator(digest);

            generator.Init(passworBT, saltBT, iterations);

            // AES-256 = 256 bits
            var keyParam = (generator.GenerateDerivedParameters("AES256", 256) as KeyParameter);
            var result = keyParam!.GetKey();

            return result;
        }

        public static byte[] CreateIV()
        {
            return RandomNumberGenerator.GetBytes(16);
        }
    }
}
