using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Model.Context
{    
    [JsonSerializable(typeof(MovieConnection))]
    public partial class MovieConnectionJsonSerializerContext : JsonSerializerContext
    {        
    }
}
