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
        private byte[] MasterKey { get; set; }

        // ------------------------------------------------------------------------

        public byte[] CreateIV
        {
            get
            {
                return RandomNumberGenerator.GetBytes(16);
            }
        }

        public byte[] CreateNonce
        {
            get
            {
                return RandomNumberGenerator.GetBytes(12);
            }
        }

        // ------------------------------------------------------------------------

        private byte[] CreateMasterKey(byte[] password, byte[] salt)
        {
            // SHA-256 기반 PBKDF2
            var digest = new Sha256Digest();
            var generator = new Pkcs5S2ParametersGenerator(digest);
            var iterations = 100_000;

            generator.Init(password, salt, iterations);

            // AES-256 = 256 bits
            var keyParam = (generator.GenerateDerivedParameters("AES256", 256) as KeyParameter);
            var result = keyParam!.GetKey();

            return result;
        }

        // HKDF
        public byte[] CreateSubKey(byte[] masterKey, byte[]? salt, string info, int keyLength)
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

        // ------------------------------------------------------------------------

        public CryptoKeySet(byte[] password, byte[] salt)
        {
            this.MasterKey = this.CreateMasterKey(password, salt);
        }

        public CryptoKeySet(XAppSettings.AppSettingsX asx, XModel.CryptoWorkOrder cwo)
        {
            this.MasterKey = this.CreateMasterKey(cwo.CryptoPassword, asx.Crypto.GetSalt);
        }

        public byte[] CreateKey(byte[] salt, string info, int keyLength)
        {
            return this.CreateSubKey(this.MasterKey, salt, info, keyLength);
        }

        public byte[] CreateKey(string info, int keyLength)
        {
            return this.CreateSubKey(this.MasterKey, null, info, keyLength);
        }
    }
}
