using System;
using System.Threading.Tasks;

using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Data
{
    public interface IDataSource : IDisposable
    {
        Task<Movie> GetMovieAsync(string title, int releaseYear);
        Task<MoviesCredits> GetCreditsForMovieAsync(int movieId);
        Task<PersonsMovieCredits> GetMovieCreditsForPersonAsync(int personId);

        int MoviesFetched { get; }
        int MovieCreditsFetched { get; }
        int PersonMoviesCreditsFetched { get; }     
    }
}