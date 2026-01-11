using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XConsole
{
    public class ConsoleWriteMessageSet
    {
        public ConsoleWriteMessage Normal { get; private set; }
        public ConsoleWriteMessage Success { get; private set; }
        public ConsoleWriteMessage Warning { get; private set; }
        public ConsoleWriteMessage Error { get; private set; }

        // --------------------------------------------------------

        public ConsoleWriteMessageSet()
        {
            this.Normal = new ConsoleWriteMessage(ConsoleColor.Gray);
            this.Success = new ConsoleWriteMessage(ConsoleColor.Green);
            this.Warning = new ConsoleWriteMessage(ConsoleColor.Yellow);
            this.Error = new ConsoleWriteMessage(ConsoleColor.Red);
        }
    }
}
