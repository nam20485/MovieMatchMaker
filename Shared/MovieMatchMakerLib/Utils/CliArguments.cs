using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace MovieMatchMakerLib.Utils
{
    public class CliArguments
    {
        private readonly string[] _args;        

        public CliArguments(string[] args)
        {
            _args = args;            
        }

        protected int GetPropertyIntValue([CallerMemberName] string propertyName = null) => int.Parse(GetValue(propertyName));
        protected string GetPropertyStringValue([CallerMemberName] string propertyName = null) => GetValue(propertyName);
        protected bool GetPropertyBoolValue([CallerMemberName] string propertyName = null) => bool.Parse(GetValue(propertyName));

        public string GetValue(string name)
        {
            for (int i = 0; i < _args.Length; i++)
            {
                var stripped = _args[i].Replace("-", string.Empty).Replace("/", string.Empty);                
                if (stripped.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    if (i + 1 >= _args.Length ||          // end of args/
                        _args[i + 1].StartsWith("--") ||
                        _args[i + 1].StartsWith("/"))
                    {
                        return "true";                        
                    }
                    else
                    {
                        // 'valued' argument 
                        return _args[i + 1];
                    }
                }
            }
            return null;            
        }

        public bool Bool(string name)
        {
            return bool.Parse(GetValue(name));
        }

        public int Int(string name)
        {
            return int.Parse(GetValue(name));
        }

        public string Str(string name)
        {
            return GetValue(name);
        }  
        

    }
}