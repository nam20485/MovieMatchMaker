using System;
using System.Collections.Generic;
using System.Text;

using TMDbLib.Objects.Movies;

namespace MovieMatchMakerLib
{
    public class MoviesCredits
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Credits Credits { get; set; }

        public class IntDictionary : Dictionary<int, MoviesCredits>
        {
        }
    }
}
