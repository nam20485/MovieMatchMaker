using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib
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
