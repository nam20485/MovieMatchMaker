using System.Text.Json.Serialization;

using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Filters
{
    [JsonDerivedType(typeof(SortFilter), "Sort")]
    [JsonDerivedType(typeof(MaxMatchingTitleWordsFilter), "MaxMatchingTitleWords")]
    [JsonDerivedType(typeof(MinConnectedRolesCountFilter), "MinConnectedRolesCount")]
    public interface IMovieConnectionListFilter
    {
        MovieConnection.List Apply(MovieConnection.List input);
    }
}
