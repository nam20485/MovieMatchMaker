using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace MovieMatchMakerLib.Utils
{
    public class ConsoleLogger : Logger
    {
        private static readonly TextWriter[] _defaultTextWriterOutputs = { Console.Out, Console.Error };

        public ConsoleLogger(LogLevel logLevel)
            : base(_defaultTextWriterOutputs, logLevel)
        {

        }
    }
}
