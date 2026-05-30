using System;
using System.Collections.Generic;
using System.Text;

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace OneFileEncryptDecrypt.XCrypto
{
    public class AES256GCM
    {
        public static byte[] EncryptNow(byte[] source, byte[] key, byte[] nonce, byte[] aad, string title, XModel.ProgressViewer? pv)
        {
            using (var outStream = new MemoryStream())
            {
                // 결과 앞에 nonce 저장
                outStream.Write(nonce, 0, nonce.Length);

                var tagSizeBits = XValue.ProcessValue.TagSizeBits;
                var aes = new AesEngine();
                var cipher = new GcmBlockCipher(aes);
                var keyParam = new KeyParameter(key);
                var parameters = new AeadParameters(keyParam, tagSizeBits, nonce, aad);

                cipher.Init(true, parameters);

                using (var inStream = new MemoryStream(source, false))
                {
                    var bufferSize = XValue.ProcessValue.BufferChunkSize;
                    var inBuffer = new byte[bufferSize];
                    var outBuffer = new byte[cipher.GetOutputSize(bufferSize)];
                    var isLoop = true;

                    pv?.Start(title, inStream.Length);

                    while (isLoop == true)
                    {
                        var readLen = inStream.Read(inBuffer, 0, inBuffer.Length);

                        if (readLen > 0)
                        {
                            var outLen = cipher.ProcessBytes(inBuffer, 0, readLen, outBuffer, 0);

                            if (outLen > 0)
                            {
                                outStream.Write(outBuffer, 0, outLen);
                            }
                        }
                        else
                        {
                            isLoop = false;
                        }

                        // 진행 표시
                        pv?.AddProgress(readLen);
                        pv?.ProgressDisplay();
                    }

                    inStream.Close();
                    pv?.Done();
                }

                var finalBuffer = new byte[cipher.GetOutputSize(0)];
                var finalLen = cipher.DoFinal(finalBuffer, 0);

                if (finalLen > 0)
                {
                    outStream.Write(finalBuffer, 0, finalLen);
                }

                var result = outStream.ToArray();

                outStream.Close();

                return result;
            }
        }

        public static byte[] DecryptNow(byte[] source, byte[] key, int nonceSize, byte[] aad, string title, XModel.ProgressViewer? pv)
        {
            using (var outStream = new MemoryStream())
            {
                var nonce = new byte[nonceSize];

                Buffer.BlockCopy(source, 0, nonce, 0, nonceSize);

                var tagSizeBits = XValue.ProcessValue.TagSizeBits;
                var aes = new AesEngine();
                var cipher = new GcmBlockCipher(aes);
                var keyParam = new KeyParameter(key);
                var parameters = new AeadParameters(keyParam, tagSizeBits, nonce, aad);
                var useSourceLen = (source.Length - nonceSize);

                cipher.Init(false, parameters);

                using (var inStream = new MemoryStream(source, nonceSize, useSourceLen, false))
                {
                    var bufferSize = XValue.ProcessValue.BufferChunkSize;
                    var inBuffer = new byte[bufferSize];
                    var outBuffer = new byte[bufferSize];
                    var isLoop = true;

                    pv?.Start(title, inStream.Length);

                    while (isLoop == true)
                    {
                        var readLen = inStream.Read(inBuffer, 0, inBuffer.Length);

                        if (readLen > 0)
                        {
                            var outLen = cipher.ProcessBytes(inBuffer, 0, readLen, outBuffer, 0);

                            if (outLen > 0)
                            {
                                outStream.Write(outBuffer, 0, outLen);
                            }
                        }
                        else
                        {
                            isLoop = false;
                        }

                        // 진행 표시
                        pv?.AddProgress(readLen);
                        pv?.ProgressDisplay();
                    }

                    inStream.Close();
                    pv?.Done();
                }

                var finalBuffer = new byte[cipher.GetOutputSize(0)];
                var finalLen = cipher.DoFinal(finalBuffer, 0);

                if (finalLen > 0)
                {
                    outStream.Write(finalBuffer, 0, finalLen);
                }

                var result = outStream.ToArray();

                outStream.Close();

                return result;
            }
        }
    }
}
