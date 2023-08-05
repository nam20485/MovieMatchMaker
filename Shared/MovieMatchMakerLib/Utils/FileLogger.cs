using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace MovieMatchMakerLib.Utils
{
    public class FileLogger : Logger
    {
        public string FilePath { get; }

        private readonly StreamWriter _fileWriter;

        public FileLogger(string filePath, LogLevel logLevel)
            : base(logLevel)
        {
            FilePath = filePath;
            _fileWriter = new StreamWriter(filePath, false);
        }

        private bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!_disposed)
            {

                if (disposing)
                {
                    _fileWriter.Flush();
                    _fileWriter.Close();
                    _fileWriter.Dispose();
                    //foreach (var textWriterOutput in TextWriterOutputs)
                    //{
                    //    textWriterOutput.Flush();
                    //    textWriterOutput.Close();
                    //    textWriterOutput.Dispose();
                    //}
                }

                _disposed = true;
            }
        }
    }
}
