using System.Collections.Generic;
using System.Text.Json.Serialization;

using MovieMatchMakerLib.TmdbApi;

namespace MovieMatchMakerLib.Model
{
    public class TvShow : Production, ITmdbLinkable
    {
        [JsonIgnore]
        public string TmdbLink => TmdbApiHelper.MakeTmdbUrl("tvshow", ApiId);

        public TvShow()
        {
            // required for deserialization
        }

        public TvShow(string name, int firstAirYear, int apiId, string posterImagePath)
            : base(name, firstAirYear, apiId, posterImagePath)
        {         
        }

        public class List : List<TvShow>
        {
            public List()
                : base()
            {
            }

            public List(IEnumerable<TvShow> items)
                : base(items)
            {
            }
        }

        public class HashSet : HashSet<TvShow>
        {
        }
    }
}