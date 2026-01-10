using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace OneFileEncryptDecrypt.XCrypto
{
    public class CryptoWork
    {
        public static byte[] CreateAES256Key(string password, string salt)
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

        public static byte[] CreateAES256IV()
        {
            return RandomNumberGenerator.GetBytes(16);
        }

        public static byte[] AES256Encrypt(byte[] key, byte[] iv, byte[] source)
        {
            var result = new List<byte>();

            if ((key.Length == 32) && (iv.Length == 16))
            {
                if (source.Length > 0)
                {
                    try
                    {
                        using (var aes = Aes.Create())
                        {
                            aes.KeySize = 256;
                            aes.Mode = CipherMode.CBC;
                            aes.Padding = PaddingMode.PKCS7;
                            aes.Key = key;
                            aes.IV = iv;

                            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                            {
                                using (var ms = new MemoryStream())
                                {
                                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                                    {
                                        cs.Write(source, 0, source.Length);
                                        cs.FlushFinalBlock();

                                        result.AddRange(ms.ToArray());

                                        cs.Clear();
                                        cs.Close();
                                    }

                                    ms.Close();
                                }
                            }

                            aes.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        CryptoWork.SilentException(ex);

                        throw;
                    }
                }
                else
                {
                    throw new ArgumentException("Empty source.");
                }
            }
            else
            {
                throw new ArgumentException("Require Key is 32 length and iv is 16 length.");
            }

            return result.ToArray();
        }

        public static byte[] AES256Decrypt(byte[] key, byte[] iv, byte[] source)
        {
            var streamList = new List<byte>();

            if ((key.Length == 32) && (iv.Length == 16))
            {
                if (source.Length > 0)
                {
                    try
                    {
                        using (var aes = Aes.Create())
                        {
                            aes.KeySize = 256;
                            aes.Mode = CipherMode.CBC;
                            aes.Padding = PaddingMode.PKCS7;
                            aes.Key = key;
                            aes.IV = iv;

                            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                            {
                                using (var ms = new MemoryStream(source))
                                {
                                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                                    {
                                        var buffer = new byte[32];
                                        var read = 0;

                                        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                                        {
                                            streamList.AddRange(buffer.Take(read));
                                        }

                                        cs.Clear();
                                        cs.Close();
                                    }

                                    ms.Close();
                                }
                            }

                            aes.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        CryptoWork.SilentException(ex);

                        throw;
                    }
                }
                else
                {
                    throw new ArgumentException("Empty source.");
                }
            }
            else
            {
                throw new ArgumentException("Require Key is 32 length and iv is 16 length.");
            }

            var result = streamList.ToArray();

            return result;
        }

        private static void SilentException(Exception ex)
        {
        }
    }
}
