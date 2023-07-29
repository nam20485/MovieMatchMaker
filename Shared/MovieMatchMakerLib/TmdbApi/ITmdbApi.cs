using System.Threading.Tasks;

using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib.TmdbApi
{
    public interface ITmdbApi
    {
        Task<ProfileImages> FetchImageDataForPerson(int personId);

        //string TmdbApiKey { get; }

        Task<Model.Movie> FetchMovieAsync(string title, int releaseYear);
        Task<Credits> FetchMovieCreditsAsync(string title, int releaseYear);
        Task<Credits> FetchMovieCreditsAsync(int movieApiId);
        Task<MovieCredits> FetchMovieCreditsForPerson(int personApiId);
        //Task<Movie> FetchConnectedMovies(string title, int releaseYear);
    }
}
