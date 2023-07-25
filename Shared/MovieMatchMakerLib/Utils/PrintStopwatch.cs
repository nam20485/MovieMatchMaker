using System;
using System.Diagnostics;

namespace MovieMatchMakerLib.Utils
{
    public class PrintStopwatch : Stopwatch
    {
        public string MessageFormat { get; set; }

        private const string _defaultMessageFormat = "{0} ({1} ms)";

        public PrintStopwatch()
            : this(_defaultMessageFormat)
        {
        }

        public PrintStopwatch(string messageFormat)
            : base()
        {
            MessageFormat = messageFormat;
        }

        public void Start(string message, bool newLine = true)
        {
            if (newLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }
            Start();
        }

        public Stopwatch StartNew(string message, bool newLine = true)
        {
            if (newLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }
            return StartNew();
        }

        public void Stop(string message)
        {
            Stop();
            Console.WriteLine(string.Format(MessageFormat, message, ElapsedMilliseconds));
            Reset();
        }
    }
}
