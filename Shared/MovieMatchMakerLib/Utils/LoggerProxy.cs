using System;
using System.Collections.Generic;
using System.Linq;
using static MovieMatchMakerLib.Utils.Logger;

namespace MovieMatchMakerLib.Utils
{
    public static class LoggerProxy
    {
        public delegate void LogRequestHandler(Logger.Level level, string message, params object[] formatArgs);

        public static event LogRequestHandler OnLogRequested;        

        public static void Log(Logger.Level level, string message, params object[] formatArgs)
        {
            OnLogRequested?.Invoke(level, message, formatArgs);
        }

        public static void Trace(string format, params object[] @params)
        {
            Log(Level.Trace, format, @params);
        }

        public static void Debug(string format, params object[] @params)
        {
            Log(Level.Debug, format, @params);
        }
        public static void Information(string format, params object[] @params)
        {
            Log(Level.Information, format, @params);
        }

        public static void Warning(string format, params object[] @params)
        {
            Log(Level.Warning, format, @params);
        }

        public static void Error(string format, params object[] @params)
        {
            Log(Level.Error, format, @params);
        }

        public static void Critical(string format, params object[] @params)
        {
            Log(Level.Critical, format, @params);
        }
    }
}
