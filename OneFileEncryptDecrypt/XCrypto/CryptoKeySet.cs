using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace OneFileEncryptDecrypt.XCrypto
{
    public class CryptoKeySet
    {
        public string KeyType { get; private set; }
        public int KeyIterations { get; private set; }
        private byte[] MasterKey { get; set; }
        public byte[] CryptoSalt { get; set; }
        public int NonceSize { get; private set; }

        // ------------------------------------------------------------------------

        public byte[] GetCryptoIV
        {
            get
            {
                return RandomNumberGenerator.GetBytes(16);
            }
        }

        public byte[] GetCryptoNonce
        {
            get
            {
                return RandomNumberGenerator.GetBytes(this.NonceSize);
            }
        }

        public byte[] GetOriginalHMACKey
        {
            get
            {
                return this.CreateKey("ORIGINALDATA", 32);
            }
        }

        public byte[] GetCryptoKey
        {
            get
            {
                return this.CreateKey("CRYPTO", 32);
            }
        }

        public byte[] GetCryptoHMACKey
        {
            get
            {
                return this.CreateKey("CRYPTODATA", 32);
            }
        }

        // ------------------------------------------------------------------------

        private byte[] CreateMasterKey(byte[] password, byte[] salt, int iterations)
        {
            // SHA-256 기반 PBKDF2
            var digest = new Sha256Digest();
            var generator = new Pkcs5S2ParametersGenerator(digest);

            generator.Init(password, salt, iterations);

            // AES-256 = 256 bits
            var keyParam = (generator.GenerateDerivedParameters("AES256", 256) as KeyParameter);
            var result = keyParam!.GetKey();

            return result;
        }

        // HKDF
        private byte[] CreateSubKey(byte[] masterKey, byte[]? salt, string info, int keyLength)
        {
            var infoUse = Encoding.UTF8.GetBytes(info);
            var digest = new Sha256Digest();
            var hkdf = new HkdfBytesGenerator(digest);

            // salt가 null이면 RFC 5869 규칙에 따라 내부에서 처리됨
            var parameters = new HkdfParameters(masterKey, salt, infoUse);
            hkdf.Init(parameters);

            var result = new byte[keyLength];
            hkdf.GenerateBytes(result, 0, keyLength);

            return result;
        }

        private byte[] CreateSalt(byte[]? salt)
        {
            return (((salt != null) && (salt.Length > 0)) ? salt : RandomNumberGenerator.GetBytes(16));
        }

        // ------------------------------------------------------------------------

        private CryptoKeySet(byte[] password, byte[]? salt, int dummyX)
        {
            var saltUse = this.CreateSalt(salt);
            var keyIterations = 100_000;

            this.KeyType = "SHA-256 PBKDF2 HKDF";
            this.KeyIterations = keyIterations;
            this.MasterKey = this.CreateMasterKey(password, saltUse, keyIterations);
            this.CryptoSalt = saltUse;
            this.NonceSize = 12;
        }

        public CryptoKeySet(XModel.CryptoWorkOrder cwo, byte[] salt) : this(cwo.CryptoPassword, salt, 0)
        {
            // Empty
        }

        public CryptoKeySet(XModel.CryptoWorkOrder cwo) : this(cwo.CryptoPassword, null, 0)
        {
            // Empty
        }

        private byte[] CreateKey(byte[] salt, string info, int keyLength)
        {
            return this.CreateSubKey(this.MasterKey, salt, info, keyLength);
        }

        private byte[] CreateKey(string info, int keyLength)
        {
            return this.CreateSubKey(this.MasterKey, null, info, keyLength);
        }
    }
}
