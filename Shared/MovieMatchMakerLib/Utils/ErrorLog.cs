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
                var s = "";
                if (! IsFirstWrite())
                {
                    s += Environment.NewLine;
                }
                s += message;
                File.AppendAllText(ErrorLogFile, s);
            }
        }

        private static bool IsFirstWrite()
        {
            return !File.Exists(ErrorLogFile) || new FileInfo(ErrorLogFile).Length == 0;
        }

        public static void Log(Exception e)
        {
            Log($"Exception:\n{e}");            
        }

        public static void Reset()
        {
            File.Delete(ErrorLogFile);
        }
    }
}
