using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace OneFileEncryptDecrypt.XCrypto
{
    public class HashWork
    {
        // Keep!
        private static byte[] CreateSHA512(byte[] source, string title, XModel.ProgressViewer? pv)
        {
            var chunkSize = XValue.ProcessValue.BufferChunkSize;
            var offset = 0;
            var hashList = new List<byte>();    

            using (var hash = SHA512.Create())
            {
                pv?.Start(title, source.Length);

                while (offset < source.Length)
                {
                    var readBytes = Math.Min(chunkSize, (source.Length - offset));

                    // chunk 단위 해시
                    hash.TransformBlock(source, offset, readBytes, null, 0);

                    offset += readBytes;

                    // 진행 표시
                    pv?.AddProgress(readBytes);
                    pv?.ProgressDisplay();
                }

                hash.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

                hashList.AddRange(hash.Hash!);

                hash.Clear();
                pv?.Done();
            }

            var result = hashList.ToArray();

            return result;
        }

        public static byte[] CreateSHA512HMAC(byte[] source, byte[] key, string title, XModel.ProgressViewer? pv)
        {
            var chunkSize = XValue.ProcessValue.BufferChunkSize;
            var offset = 0;
            var hashList = new List<byte>();

            using (var hash = new HMACSHA512(key))
            {
                pv?.Start(title, source.Length);

                while (offset < source.Length)
                {
                    var readBytes = Math.Min(chunkSize, (source.Length - offset));

                    // chunk 단위 해시
                    hash.TransformBlock(source, offset, readBytes, null, 0);

                    offset += readBytes;

                    // 진행 표시
                    pv?.AddProgress(readBytes);
                    pv?.ProgressDisplay();
                }

                hash.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

                hashList.AddRange(hash.Hash!);

                hash.Clear();
                pv?.Done();
            }

            var result = hashList.ToArray();

            return result;
        }

        public static string ConvertHashText(byte[] hashSource)
        {
            return Regex.Replace(BitConverter.ToString(hashSource), "[^0-9A-Za-z]", string.Empty, RegexOptions.IgnoreCase).Trim().ToLower();
        }
    }
}
