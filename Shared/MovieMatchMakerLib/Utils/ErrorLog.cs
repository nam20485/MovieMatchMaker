using System;
using System.IO;

namespace MovieMatchMakerLib.Utils
{
    public static class ErrorLog
    {
        public static string LogFile { get; set; } = "error.txt";

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
                File.AppendAllText(LogFile, s);
            }
        }

        private static bool IsFirstWrite()
        {
            return !File.Exists(LogFile) || new FileInfo(LogFile).Length == 0;
        }

        public static void Log(Exception e)
        {
            Log(e.ToString());            
        }

        public static void Reset()
        {
            File.Delete(LogFile);
        }
    }
}
