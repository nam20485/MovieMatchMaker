﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

using MovieMatchMakerLib.TmdbApi;

namespace MovieMatchMakerLib.Model
{
    public class Movie : Production
    {
        [JsonIgnore]
        public override string TmdbLink => TmdbApiHelper.MakeTmdbUrl("movie", ApiId);

        public Movie()
        {
        }

        public Movie(string title, int releaseYear, int movieId, string posterImagePath)
            : base(title, releaseYear, movieId, posterImagePath)
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
