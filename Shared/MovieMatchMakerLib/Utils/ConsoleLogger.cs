using System;
using System.IO;

namespace MovieMatchMakerLib.Utils
{
    public class ConsoleLogger : Logger
    {
        private static readonly TextWriter[] _defaultTextWriterOutputs = { Console.Out, Console.Error };
        
        public ConsoleLogger()
            : base()
        {
        }

        public ConsoleLogger(Level logLevel)
            : base(_defaultTextWriterOutputs, logLevel)
        {

        }
    }
}
