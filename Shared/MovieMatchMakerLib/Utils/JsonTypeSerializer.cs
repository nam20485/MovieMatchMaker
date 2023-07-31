using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public static class JsonTypeSerializer<TType>
    {
        //public TType Instance { get; }

        //private readonly JsonSerializerOptions _jsonSerializerOptions;

        //public JsonTypeSerializer(TType instance, JsonSerializerOptions options)
        //{
        //    Instance = instance;
        //    _jsonSerializerOptions = options;
        //}

        //public JsonTypeSerializer(TType instance)
        //    : this(instance, GlobalSerializerOptions.Options)
        //{            
        //}

        //public bool LoadFromFile(string filePath)
        //{
        //    return false;
        //}

        //public bool SaveToFile(string filePath)
        //{
        //    return false;
        //}

        //public string ToJson()
        //{
        //    return JsonSerializer.Serialize(Instance);
        //}

        //public void FromJson(string json)
        //{
        //    Instance = JsonSerializer.Deserialize<TType>(json);
        //}
    }
}
