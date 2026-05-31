using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace OneFileEncryptDecrypt.XWork
{
    public class FileWork
    {
        public static byte[] GetFileByte(string filePath, string title, XModel.ProgressViewer? pv)
        {
            var bufferSize = XValue.ProcessValue.BufferChunkSize;
            var buffer = new byte[bufferSize];
            var allBytes = new List<byte>();
            var isLoop = true;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                pv?.ProgressStart(title, fs.Length);

                while (isLoop == true)
                {
                    var readBytes = fs.Read(buffer, 0, buffer.Length);

                    if (readBytes > 0)
                    {
                        // 읽은 데이터 누적
                        // buffer.Take(readBytes)
                        allBytes.AddRange(buffer[..readBytes]);

                        pv?.AddProgress(readBytes);
                        pv?.ProgressDisplay();
                    }
                    else
                    {
                        isLoop = false;
                    }
                }

                fs.Close();
                pv?.ProgressDone();
            }

            var result = allBytes.ToArray();

            return result;
        }

        public static void WriteFileByte(byte[] source, string saveFilePath, string title, XModel.ProgressViewer? pv)
        {
            var chunkSize = XValue.ProcessValue.BufferChunkSize;
            var offset = 0;

            using (var fs = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                pv?.ProgressStart(title, source.Length);

                while (offset < source.Length)
                {
                    var readBytes = Math.Min(chunkSize, (source.Length - offset));

                    fs.Write(source, offset, readBytes);

                    offset += readBytes;

                    // 진행 표시
                    pv?.AddProgress(readBytes);
                    pv?.ProgressDisplay();
                }

                fs.Close();
                pv?.ProgressDone();
            }
        }

        // TODO : ZIP 압축과 해제는 나중에~~ Stream하게 하자..... 나중에~ 나중에 ㅎㅎㅎ
        public static void ZIPCompression(string sourceDirectoryPath, string zipFilePath, string title, XModel.ProgressViewer? pv)
        {
            pv?.ProgressStart(title, 100);
            pv?.ProgressDisplay();

            ZipFile.CreateFromDirectory(sourceDirectoryPath, zipFilePath, CompressionLevel.NoCompression, false);

            pv?.AddProgress(100);
            pv?.ProgressDisplay();
            pv?.ProgressDone();
        }

        public static void ZIPExtract(string sourceFIlePath, string extractDirectoryPath, string title, XModel.ProgressViewer? pv)
        {
            pv?.ProgressStart(title, 100);
            pv?.ProgressDisplay();

            ZipFile.ExtractToDirectory(sourceFIlePath, extractDirectoryPath, true);

            pv?.AddProgress(100);
            pv?.ProgressDisplay();
            pv?.ProgressDone();
        }

        // https://en.wikipedia.org/wiki/List_of_file_signatures
        private static string GetFileMagicByte(string filePath, int startPosition, int readSize)
        {
            var byteX = new byte[readSize];

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Seek(startPosition, SeekOrigin.Begin);
                fs.ReadExactly(byteX);
                fs.Close();
            }

            var byteXText = BitConverter.ToString(byteX);
            var result = Regex.Replace(byteXText, "[^0-9A-Za-z]", string.Empty, RegexOptions.IgnoreCase).Trim().ToUpper();

            return result;
        }

        public static bool IsZIPFileMagicByte(string filePath)
        {
            return (FileWork.GetFileMagicByte(filePath, 0, 4) == "504B0304");
        }
    }
}
