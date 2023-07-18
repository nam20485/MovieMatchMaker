﻿using System.Collections.Generic;

namespace MovieMatchMakerLib.Model
{
    public class Movie : Production
    {
        public Movie(string title, int releaseYear, int movieId)
            : base(title, releaseYear, movieId)
        {
        }

        public class List : List<Movie>
        {
            public List()
                : base()
            {
            }

            public List(IEnumerable<Movie> items)
                : base(items)
            {
            }
        }

        public class HashSet : HashSet<Movie>
        {
        }
    }
}
