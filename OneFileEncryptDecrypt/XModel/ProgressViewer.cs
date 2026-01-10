using System;
using System.Collections.Generic;
using System.Text;

namespace OneFileEncryptDecrypt.XModel
{
    public class ProgressViewer
    {
        public string Title { get; private set; }
        public decimal TotalCount { get; private set; }
        public decimal NowCount { get; private set; }

        // ---------------------------------------

        private decimal CalcPercent(decimal totalCount, decimal nowCount)
        {
            return ((totalCount > 0m) ? ((nowCount / totalCount) * 100m) : 0m);
        }

        // ---------------------------------------

        public ProgressViewer()
        {
            this.Title = string.Empty;
            this.TotalCount = 0m;
            this.NowCount = 0m;
        }

        public void Start(string title, decimal totalCount)
        {
            this.Title = title;
            this.TotalCount = totalCount;
            this.NowCount = 0m;
        }

        public void ChangeTotalCount(decimal totalCount)
        {
            this.TotalCount = totalCount;
        }

        public decimal GetPercent()
        {
            return this.CalcPercent(this.TotalCount, this.NowCount);
        }

        public void AddProgress(decimal nowCount)
        {
            this.NowCount += nowCount;
        }

        public void ProgressDisplay()
        {
            var title = this.Title;
            var totalCount = this.TotalCount;
            var nowCount = this.NowCount;
            var percent = this.CalcPercent(totalCount, nowCount);

            Console.CursorLeft = 0;
            Console.Out.Write($"[{title}] {nowCount:N0} / {totalCount:N0} ({percent:F1}%)");
            //Thread.Sleep(50);
        }

        public void Done()
        {
            Console.Out.WriteLine($" - Done");
        }
    }
}
