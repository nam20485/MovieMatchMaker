using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Model.Context
{
    //[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
    [JsonSerializable(typeof(Name))]
    [JsonSerializable(typeof(MovieConnection))]
    [JsonSerializable(typeof(ConnectedRole))]
    [JsonSerializable(typeof(Movie))]
    [JsonSerializable(typeof(ConnectedRole.List), TypeInfoPropertyName = "ConnectedRoleList")]
    [JsonSerializable(typeof(MovieConnection.List), TypeInfoPropertyName = "MovieConnectionList"/*, GenerationMode = JsonSourceGenerationMode.Serialization*/)]
    public partial class MovieConnectionListJsonSerializerContext : JsonSerializerContext
    {        
    }
}
