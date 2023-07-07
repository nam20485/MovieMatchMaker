using System.Threading.Tasks;

using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib
{
    public interface IDataSource
    {
        Task<Movie> GetMovieAsync(string title, int releaseYear);       
        Task<MoviesCredits> GetCreditsForMovieAsync(int movieId);
        Task<PersonsMovieCredits> GetMovieCreditsForPersonAsync(int personId);
        //Task UpdateMovieCreditsAsync(string title, int releaseYear);
    }
}