using System.Threading.Tasks;

using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib.TmdbApi
{
    public interface ITmdbApi
    {
        Task<Model.Movie> FetchMovieAsync(string title, int releaseYear);
        Task<Credits> FetchMovieCreditsAsync(string title, int releaseYear);
        Task<Credits> FetchMovieCreditsAsync(int movieApiId);
        Task<MovieCredits> FetchMovieCreditsForPerson(int personApiId);
        //Task<Movie> FetchConnectedMovies(string title, int releaseYear);
    }
}
