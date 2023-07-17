using System.Collections.Generic;

namespace MovieMatchMakerLib.Model
{
    public class Movie : Production
    {

        public Movie(string title, int releaseYear, int movieId)
            : base(title, releaseYear, movieId)
        {
        }

        public class List : List<Movie> { }

    }
}
