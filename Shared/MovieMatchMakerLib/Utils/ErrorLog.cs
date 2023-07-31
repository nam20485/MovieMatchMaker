using System;
using System.IO;

namespace MovieMatchMakerLib.Utils
{
    public static class ErrorLog
    {
        public const string ErrorLogFile = "error.txt";

        private static readonly object _writeLock = new();

        public static void Log(string message)
        {
            lock (_writeLock)
            {
                File.AppendAllText(ErrorLogFile, message);
            }
        }

        public static void Log(Exception e)
        {
            Log(e.ToString());
        }

        public static void Reset()
        {
            File.Delete(ErrorLogFile);
        }
    }
}
