using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Model.Context
{

    [JsonSerializable(typeof(MovieConnection.List)/*, TypeInfoPropertyName = "MovieMatchMakerLib.Model.MovieConnection.List"*/)]
    public partial class MovieConnectionListSerializerContext : JsonSerializerContext
    {
    }
}
