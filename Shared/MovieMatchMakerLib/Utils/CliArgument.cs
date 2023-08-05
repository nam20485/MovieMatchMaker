using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class CliArgument<TType>
    {
        public string Name { get; }
        public TType Value { get; set; }

        private readonly TType _default;

        public CliArgument(string name)
        {
            Name = name;
        }

        public CliArgument(string name, TType @default)
            : this(name)
        {            
            _default = @default;
        }
    }
}
