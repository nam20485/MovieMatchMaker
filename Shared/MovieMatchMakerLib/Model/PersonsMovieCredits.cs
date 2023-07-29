using System.Collections.Generic;
using System.Text.Json.Serialization;

using TMDbLib.Objects.People;

namespace MovieMatchMakerLib.Model
{
    public class PersonsMovieCredits
    {
        public int PersonId { get; set; }
        public MovieCredits MovieCredits { get; set; }    
        public string ProfileImagePath { get; set; }
        [JsonIgnore]
        public int Id { get; set; }

        public class IntDictionary : Dictionary<int, PersonsMovieCredits>
        {
        }
    }
}
