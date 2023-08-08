using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerApp
{
    internal class AppCliArguments
    {
        public IEnumerable<CliArgument<dynamic>> Arguments { get; set; }        

        public CliArgument<bool> Threaded {  get; set; }
        public CliArgument<bool> ContinueExisting { get; set; }
        public CliArgument<int> Degree { get; set; }
        public CliArgument<string> File { get; set; }
        public CliArgument<string> Title { get; set; }
        public CliArgument<int> ReleaseYear { get; set; }

        private readonly IEnumerable<string> _args;

        public AppCliArguments(IEnumerable<string> args)
        {
            _args = args;
        }
    }
}
