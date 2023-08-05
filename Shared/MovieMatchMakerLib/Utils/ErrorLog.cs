using System;
using System.IO;

namespace MovieMatchMakerLib.Utils
{
    public static class ErrorLog
    {
        public static string File { get; set; } = "error.txt";

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
                System.IO.File.AppendAllText(File, s);
            }
        }

        private static bool IsFirstWrite()
        {
            return !System.IO.File.Exists(File) || new FileInfo(File).Length == 0;
        }

        public static void Log(Exception e)
        {
            Log(e.ToString());            
        }

        public static void Reset()
        {
            System.IO.File.Delete(File);
        }
    }
}
