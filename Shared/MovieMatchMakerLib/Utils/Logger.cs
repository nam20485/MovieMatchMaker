using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace MovieMatchMakerLib.Utils
{
    public class Logger : IDisposable
    {
        public string FilePath { get; private set; }

        public LogLevel MinimumLogLevel { get; set; }

        public List<TextWriter> TextWriterOutputs { get; }

        public string MessageFormat { get; set; }

        private const string DefaultMessageFormat = "[{0}] {1} {2}";
        private const string DefaultFilePath = "./log.txt";
        private const LogLevel DefaultLogLevel = LogLevel.Warning;

        private readonly TextWriter[] _defaultTextWriterOutputs = { Console.Out, Console.Error };        

        private readonly RequestProcessingLoopThread<LogMessageRequest> _logMessagesLoopThread;

        private bool disposedValue;

        public Logger()
            : this(DefaultFilePath, DefaultLogLevel)
        {
        }

        public Logger(string filepath, LogLevel logLevel)
        {
            FilePath = filepath;
            MinimumLogLevel = logLevel;
            MessageFormat = DefaultMessageFormat;
            TextWriterOutputs = new List<TextWriter>(_defaultTextWriterOutputs);            
            _logMessagesLoopThread = new RequestProcessingLoopThread<LogMessageRequest>(LogMessageRequestFunc);
        }

        private async Task LogMessageRequestFunc(LogMessageRequest request)
        {
            var message = FormatMessage(request);
            foreach (var textWriterOutput in TextWriterOutputs)
            {                
                await textWriterOutput.WriteLineAsync(message);
            }            
        }

        public void Start()
        {
            _logMessagesLoopThread.StartProcessingRequests();
        }

        public void Stop()
        {
            _logMessagesLoopThread.StopProcessingRequests();
        }

        public void Log(LogLevel logLevel, string message)
        {
            if (logLevel >= MinimumLogLevel)
            {                
                _logMessagesLoopThread.AddRequest(new LogMessageRequest(DateTime.UtcNow, logLevel, message));
            }
        }

        public void Log(LogLevel logLevel, string format, params object[] @params)
        {
            Log(logLevel, string.Format(format, @params));
        }       

        private string FormatMessage(LogMessageRequest request)
        {
            return string.Format(MessageFormat, request.LogLevel, request.TimeStamp, request.Message);
        }        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();
                }
               
                disposedValue = true;
            }
        }       

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private struct LogMessageRequest
        {
            public DateTime TimeStamp;
            public LogLevel LogLevel;
            public string Message;

            public LogMessageRequest(DateTime timeStamp, LogLevel level, string message)
            {
                TimeStamp = timeStamp;
                LogLevel = level;
                Message = message;
            }
        }
    }
}
