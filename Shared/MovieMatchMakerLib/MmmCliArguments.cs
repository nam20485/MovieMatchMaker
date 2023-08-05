using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public class MmmCliArguments : CliArguments
    {
        public bool BuildData => GetPropertyBoolValue();
        public bool BuildConnections => GetPropertyBoolValue();
        public bool Threaded => GetPropertyBoolValue();
        public bool ContinueExisting => GetPropertyBoolValue();
        public string File => GetPropertyStringValue();

        public string Title => GetPropertyStringValue();

        public int ReleaseYear => GetPropertyIntValue();
        public int Degree => GetPropertyIntValue();

        //--build-data --title "Dark City" --releaseYear 1998 --degree 1 --threaded true --file ./movie-data.json --continue false

        public MmmCliArguments(string[] args) 
            : base(args)
        {
            
        }      
    }
}
