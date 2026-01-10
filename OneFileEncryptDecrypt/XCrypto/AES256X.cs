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

        public static byte[] EncryptNow(byte[] key, byte[] iv, byte[] source, string title, XModel.ProgressViewer pv)
        {
            var chunkSize = XValue.ProcessValue.BufferChunkSize;
            var offset = 0;
            var result = new List<byte>();

            if (source.Length > 0)
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
                                pv.Start(title, source.Length);

                                while (offset < source.Length)
                                {
                                    var readBytes = Math.Min(chunkSize, (source.Length - offset));

                                    cs.Write(source, offset, readBytes);

                                    offset += readBytes;

                                    // 진행 표시
                                    pv.AddProgress(readBytes);
                                    pv.ProgressDisplay();
                                }

                                cs.FlushFinalBlock();

                                result.AddRange(ms.ToArray());

                                cs.Clear();
                                cs.Close();
                                pv.Done();
                            }

                            ms.Close();
                        }
                    }

                    aes.Clear();
                }
            }

            return result.ToArray();
        }

        public static byte[] DecryptNow(byte[] key, byte[] iv, byte[] source, string title, XModel.ProgressViewer pv)
        {
            var bufferSize = XValue.ProcessValue.BufferChunkSize;
            var buffer = new byte[bufferSize];
            var totalReadBytes = 0;
            var streamList = new List<byte>();
            var isLoop = true;

            if (source.Length > 0)
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
                                // 여긴 복호화니, 들어올 때 크기랑 복호화 후 크기가 다를것임
                                // 그래서 마지막에 전체 크기로 바꿔준다 ㅎㅎㅎ
                                pv.Start(title, source.Length);

                                while (isLoop == true)
                                {
                                    var readBytes = cs.Read(buffer, 0, buffer.Length);

                                    totalReadBytes += readBytes;

                                    if (readBytes > 0)
                                    {
                                        // buffer.Take(readBytes)
                                        streamList.AddRange(buffer[..readBytes]);

                                        pv.AddProgress(readBytes);
                                        pv.ProgressDisplay();
                                    }
                                    else
                                    {
                                        pv.ChangeTotalCount(totalReadBytes);
                                        pv.ProgressDisplay();

                                        isLoop = false;
                                    }
                                }

                                cs.Clear();
                                cs.Close();
                                pv.Done();
                            }

                            ms.Close();
                        }
                    }

                    aes.Clear();
                }
            }

            var result = streamList.ToArray();

            return result;
        }
    }
}
