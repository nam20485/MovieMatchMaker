using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MovieMatchMakerLib.Utils
{
    public class CliArguments
    {
        private readonly string[] _args;        

        public CliArguments(string[] args)
        {
            _args = args;
            if (args.Length == 0)
            {
                throw new EmptyArgumentsException();
            }
        }

        public T GetPropertyArgumentValue<T>([CallerMemberName] string propertyName = null) => GetValue<T>(propertyName);

        public string GetValue(string name)
        {
            for (int i = 0; i < _args.Length; i++)
            {
                var stripped = _args[i].Replace("-", string.Empty).Replace("/", string.Empty);                
                if (stripped.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    if (i + 1 >= _args.Length ||        // end of args
                        _args[i + 1].StartsWith("--") ||
                        _args[i + 1].StartsWith("/"))
                    {
                        // "value-less"/single argument
                        return "true";                        
                    }
                    else
                    {
                        // 'valued'/pair argument 
                        return _args[i + 1];
                    }
                }
            }

            throw new ArgumentNotFoundException($"Argument with name {name} not found");           
        }

        public T GetValue<T>(string name)
        {
            if (GetValue(name) is string value)
            {
                return Convert.To<T>(value);
            }
            else
            {
                return default;
            }
        }        

        [Serializable]
        private class ArgumentNotFoundException : Exception
        {
            public ArgumentNotFoundException()
            {
            }

            public ArgumentNotFoundException(string message) : base(message)
            {
            }

            public ArgumentNotFoundException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ArgumentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        private class EmptyArgumentsException : Exception
        {
            public EmptyArgumentsException()
            {
            }

            public EmptyArgumentsException(string message) : base(message)
            {
            }

            public EmptyArgumentsException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected EmptyArgumentsException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}