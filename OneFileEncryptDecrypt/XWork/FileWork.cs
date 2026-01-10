using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XWork
{
    public class FileWork
    {
        public static byte[] GetFileByte(string filePath, string title, XModel.ProgressViewer pv)
        {
            var bufferSize = XValue.ProcessValue.BufferChunkSize;
            var buffer = new byte[bufferSize];
            var allBytes = new List<byte>();
            var isLoop = true;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                pv.Start(title, fs.Length);

                while (isLoop == true)
                {
                    var readBytes = fs.Read(buffer, 0, buffer.Length);

                    if (readBytes > 0)
                    {
                        // 읽은 데이터 누적
                        // buffer.Take(readBytes)
                        allBytes.AddRange(buffer[..readBytes]);

                        pv.AddProgress(readBytes);
                        pv.ProgressDisplay();
                    }
                    else
                    {
                        isLoop = false;
                    }
                }

                fs.Close();
                pv.Done();
            }

            var result = allBytes.ToArray();

            return result;
        }

        public static void WriteFileByte(byte[] source, string saveFilePath, string title, XModel.ProgressViewer pv)
        {
            var chunkSize = XValue.ProcessValue.BufferChunkSize;
            var offset = 0;

            using (var fs = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                pv.Start(title, source.Length);

                while (offset < source.Length)
                {
                    var readBytes = Math.Min(chunkSize, (source.Length - offset));

                    fs.Write(source, offset, readBytes);

                    offset += readBytes;

                    // 진행 표시
                    pv.AddProgress(readBytes);
                    pv.ProgressDisplay();
                }

                fs.Close();
                pv.Done();
            }
        }
    }
}
