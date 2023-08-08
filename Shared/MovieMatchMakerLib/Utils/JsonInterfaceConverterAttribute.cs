using System;
using System.Text.Json.Serialization;

namespace MovieMatchMakerLib.Utils
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class JsonInterfaceConverterAttribute : JsonConverterAttribute
    {
        public JsonInterfaceConverterAttribute(Type converterType)
            : base(converterType)
        {
            // do nothing
        }
    }
}
