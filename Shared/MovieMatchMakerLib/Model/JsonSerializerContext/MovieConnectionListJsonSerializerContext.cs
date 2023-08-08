using System.Text.Json.Serialization;

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
