using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace MovieMatchMakerLib.Utils
{
    public class Logger : IDisposable
    {
        public LogLevel MinimumLogLevel { get; set; }

        public IEnumerable<TextWriter> TextWriterOutputs { get; protected set; }

        public string MessageFormat { get; set; }

        private const string DefaultMessageFormat = "[{0}] {1} {2}";
        private const LogLevel DefaultLogLevel = LogLevel.Warning;        

        private readonly RequestProcessingLoopThread<LogMessageRequest> _logMessagesLoopThread;

        private bool disposedValue;

        protected Logger(LogLevel logLevel)
        {
            MinimumLogLevel = logLevel;
        }

        public Logger(IEnumerable<TextWriter> outputs, LogLevel logLevel)
            : this(logLevel)
        {
            MessageFormat = DefaultMessageFormat;
            TextWriterOutputs = outputs;
            _logMessagesLoopThread = new RequestProcessingLoopThread<LogMessageRequest>(LogMessageRequestFunc);
        }

        public Logger(IEnumerable<TextWriter> outputs)
            : this(outputs, DefaultLogLevel)
        {
        }        

        private async Task LogMessageRequestFunc(LogMessageRequest request)
        {
            var message = FormatMessage(request);
            foreach (var textWriterOutput in TextWriterOutputs)
            {                
                await textWriterOutput.WriteLineAsync(message);
                await textWriterOutput.FlushAsync();                
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
                    // TODO: will Dispose()'ing Console.Out throw an exception
                    foreach (var textWriterOutput in TextWriterOutputs)
                    {
                        textWriterOutput.Flush();
                        textWriterOutput.Close();
                        textWriterOutput.Dispose();
                    }
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

        private struct TextWriterFileMapping
        {
            public IEnumerable<TextWriter> TextWriters { get; set; }
            public string Filename { get; set; }
        }
    }
}
