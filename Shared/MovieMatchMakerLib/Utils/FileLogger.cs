using System.IO;

namespace MovieMatchMakerLib.Utils
{
    public class FileLogger : Logger
    {
        public string FilePath { get; }

        private const string DefaultFilePath = "./log.txt";        

        public FileLogger(Level logLevel, string filePath)
            : base(logLevel)
        {
            FilePath = filePath;
            TextWriterOutputs.Add(new StreamWriter(filePath, false));
        }

        public FileLogger(Level logLevel)
            : this(logLevel, DefaultFilePath)
        {           
        }

        public FileLogger()
            : this(DefaultLogLevel)
        {
        }

        //private bool _disposed = false;
        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);

        //    if (!_disposed)
        //    {

        //        if (disposing)
        //        {
        //            //Stop();
        //            _fileWriter.Flush();
        //            _fileWriter.Close();
        //            _fileWriter.Dispose();
        //            //foreach (var textWriterOutput in TextWriterOutputs)
        //            //{
        //            //    textWriterOutput.Flush();
        //            //    textWriterOutput.Close();
        //            //    textWriterOutput.Dispose();
        //            //}
        //        }

        //        _disposed = true;
        //    }
        //}
    }
}
