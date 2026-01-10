using System.CommandLine;

namespace OneFileEncryptDecrypt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/syntax
            // https://learn.microsoft.com/ko-kr/dotnet/standard/commandline/how-to-parse-and-invoke
            var rc = new RootCommand("One file encrypt and decrypt work.");
            rc.Add(XCommand.CryptoCommand.CreateCommand("encrypt", "Encrypt", XWork.EncryptWork.ExecuteNow));
            rc.Add(XCommand.CryptoCommand.CreateCommand("decrypt", "Decrypt", XWork.DecryptWork.ExecuteNow));

            var pr = rc.Parse(args);
            //var pr = rc.Parse("encrypt --key helloworld --file D:\\Download\\Dummy\\IMG_2819.jpg");
            //var pr = rc.Parse("encrypt -k helloworld -f D:\\Download\\Dummy\\Hello.txt");

            pr.Invoke();
        }
    }
}



