using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XConsole
{
    public class ConsoleWriteMessage
    {
        private ConsoleColor TextColor { get; set; }

        // ------------------------------------------------------------

        private void MessageWork(ConsoleColor textColor, List<string> message, bool isAndEmptyLine)
        {
            if (message.Count > 0)
            {
                Console.ForegroundColor = textColor;
                Console.Out.Write(string.Join(Environment.NewLine, message));

                if (isAndEmptyLine == true)
                {
                    Console.Out.WriteLine(string.Empty);
                }

                Console.ResetColor();
            }
        }

        // ------------------------------------------------------------

        public ConsoleWriteMessage(ConsoleColor textColor)
        {
            this.TextColor = textColor;
        }

        public void MessageNow(string message, bool isAndEmptyLine)
        {
            this.MessageWork(
                this.TextColor,
                new List<string>() 
                {
                    message
                }, 
                isAndEmptyLine
            );
        }

        public void MessageNow(string message)
        {
            this.MessageWork(
                this.TextColor,
                new List<string>()
                {
                    message
                },
                false
            );
        }

        public void MessageNow(List<string> message, bool isAndEmptyLine)
        {
            this.MessageWork(this.TextColor, message, isAndEmptyLine);
        }

        public void MessageNow(List<string> message)
        {
            this.MessageWork(this.TextColor, message, false);
        }
    }
}
