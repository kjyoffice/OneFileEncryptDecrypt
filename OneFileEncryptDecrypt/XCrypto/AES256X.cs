using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OneFileEncryptDecrypt.XCrypto
{
    public class AES256X
    {
        private static void CommonWork(byte[] key, byte[] iv, Aes aes)
        {
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;
        }

        public static byte[] EncryptNow(byte[] key, byte[] iv, byte[] source)
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
                            AES256X.CommonWork(key, iv, aes);

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
                        AES256X.SilentException(ex);

                        throw;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Require Key is 32 length and iv is 16 length.");
            }

            return result.ToArray();
        }

        public static byte[] DecryptNow(byte[] key, byte[] iv, byte[] source)
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
                            AES256X.CommonWork(key, iv, aes);

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
                        AES256X.SilentException(ex);

                        throw;
                    }
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
