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
        public static string CreateSHA512(byte[] source, string title, XModel.ProgressViewer pv)
        {
            var chunkSize = XValue.ProcessValue.BufferChunkSize;
            var offset = 0;
            var result = string.Empty;

            using (var hash = SHA512.Create())
            {
                pv.Start(title, source.Length);

                while (offset < source.Length)
                {
                    var readBytes = Math.Min(chunkSize, (source.Length - offset));

                    // chunk 단위 해시
                    hash.TransformBlock(source, offset, readBytes, null, 0);

                    offset += readBytes;

                    // 진행 표시
                    pv.AddProgress(readBytes);
                    pv.ProgressDisplay();
                }

                hash.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

                var rawText = BitConverter.ToString(hash.Hash!);

                result = Regex.Replace(rawText, "[^0-9A-Za-z]", string.Empty, RegexOptions.IgnoreCase).Trim().ToLower();

                hash.Clear();
                pv.Done();
            }

            return result;
        }
    }
}
