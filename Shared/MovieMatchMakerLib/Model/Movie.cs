using System.Collections.Generic;
using MovieMatchMakerLib.TmdbApi;

namespace MovieMatchMakerLib.Model
{
    public class Movie : Production, ITmdbLinkable
    {
        public string TmdbLink => TmdbApiHelper.MakeTmdbUrl("movie", MovieId);

        public Movie(string title, int releaseYear, int movieId, string posterImagePath)
            : base(title, releaseYear, movieId, posterImagePath)
        {
        }

        public string MakePosterImagePath(TmdbApiHelper.PosterImageSize posterImageSize)
        {
            return TmdbApiHelper.MakeMoviePosterImagePath(posterImageSize, PosterImagePath);
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
