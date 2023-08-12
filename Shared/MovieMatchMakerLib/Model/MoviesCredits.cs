using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using TMDbLib.Objects.Movies;

namespace MovieMatchMakerLib.Model
{
    public class MoviesCredits
    {
        public int MovieId { get; set; }
        public Credits Credits { get; set; }
        //[JsonIgnore]
        //public int Id { get; set; }

        public class IntDictionary : ConcurrentDictionary<int, MoviesCredits>
        {
        }

        public class StringDictionary : ConcurrentDictionary<string, MoviesCredits> { }
    }
}
