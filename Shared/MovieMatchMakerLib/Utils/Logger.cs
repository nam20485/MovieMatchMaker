using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class Logger : IDisposable
    {
        public enum Level
        {
            Trace = 0,
            Debug = 1,
            Information = 2,
            Warning = 3,
            Error = 4,
            Critical = 5,
            None = 6,
        }

        public Level MinimumLogLevel { get; set; }

        public List<TextWriter> TextWriterOutputs { get; protected set; }

        public string MessageFormat { get; set; }

        private const string DefaultMessageFormat = "[{0}] {1} {2}";
        protected const Level DefaultLogLevel = Level.Warning;        

        private readonly RequestProcessingLoopThread<LogMessageRequest> _logMessagesLoopThread;

        private bool disposedValue;       

        protected Logger(Level logLevel)
        {
            TextWriterOutputs = new ();
            MinimumLogLevel = logLevel;
            MessageFormat = DefaultMessageFormat;           
            _logMessagesLoopThread = new RequestProcessingLoopThread<LogMessageRequest>(LogMessageRequestFunc);
        }
        protected Logger()
           : this(DefaultLogLevel)
        {
        }

        public Logger(IEnumerable<TextWriter> outputs, Level logLevel)
            : this(logLevel)
        {
            TextWriterOutputs.AddRange(outputs);
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

        public void Log(Level logLevel, string message)
        {
            if (logLevel >= MinimumLogLevel)
            {                
                _logMessagesLoopThread.AddRequest(new LogMessageRequest(DateTime.UtcNow, logLevel, message));
            }
        }

        public void Log(Level logLevel, string format, params object[] @params)
        {
            Log(logLevel, string.Format(format, @params));
        }

        public void Trace(string format, params object[] @params)
        {
            Log(Level.Trace, format, @params);
        }

        public void Debug(string format, params object[] @params)
        {
            Log(Level.Debug, format, @params);
        }

        public void Information(string format, params object[] @params)
        {
            Log(Level.Information, format, @params);
        }

        public void Warning(string format, params object[] @params)
        {
            Log(Level.Warning, format, @params);
        }

        public void Error(string format, params object[] @params)
        {
            Log(Level.Error, format, @params);
        }

        public void Critical(string format, params object[] @params)
        {
            Log(Level.Critical, format, @params);
        }

        //public void None(string format, params object[] @params)
        //{
        //    Log(Level.None, format, @params);
        //}       

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
                    // Calling Dispose() on Console.{Out,Error} does not seem to throw an exception
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
            public Level LogLevel;
            public string Message;

            public LogMessageRequest(DateTime timeStamp, Level level, string message)
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