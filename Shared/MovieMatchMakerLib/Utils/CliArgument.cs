using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class CliArgument<TType>
    {
        public string Name { get; set; }
        public TType Value { get; set; }
    }
}
