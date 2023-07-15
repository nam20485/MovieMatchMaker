using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using MovieMatchMakerLib.Filters;

namespace MovieMatchMakerLib.Utils
{
    public class MovieConnectionListFilterConverter : JsonConverter<IMovieConnectionListFilter>
    {
        public override IMovieConnectionListFilter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert == typeof(SortFilter))
            {
                return JsonSerializer.Deserialize<SortFilter>(ref reader, options);
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, IMovieConnectionListFilter value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    JsonSerializer.Serialize(writer, null as IMovieConnectionListFilter, options); 
                    break;
                default:
                    var type = value.GetType();
                    JsonSerializer.Serialize(writer, value, type, options); 
                    break;
            }
        }
    }
}
