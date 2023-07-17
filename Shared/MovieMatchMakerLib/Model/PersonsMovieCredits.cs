using System.Collections.Generic;

using TMDbLib.Objects.People;

namespace MovieMatchMakerLib.Model
{
    public class PersonsMovieCredits
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public MovieCredits MovieCredits { get; set; }

        public class IntDictionary : Dictionary<int, PersonsMovieCredits>
        {
        }
    }
}
