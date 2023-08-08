using System.Text.Json.Serialization;

using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.MovieData
{
    [JsonSerializable(typeof(PersonsMovieCredits.IntDictionary), TypeInfoPropertyName = "PersonsMovieCredits_IntDictionary")]
    [JsonSerializable(typeof(MoviesCredits.IntDictionary), TypeInfoPropertyName = "MoviesCredits_IntDictionary")]
    [JsonSerializable(typeof(JsonFileCache))]
    public partial class JsonFileCacheSerializationContext : JsonSerializerContext
    {
    }
}
