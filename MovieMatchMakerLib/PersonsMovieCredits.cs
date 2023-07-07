using System;
using System.Collections.Generic;
using System.Text;

using TMDbLib.Objects.People;

namespace MovieMatchMakerLib
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
