using System.Threading.Tasks;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.TmdbApi;

namespace MovieMatchMakerLib.Data
{
    public class ApiDataSource : IDataSource
    {
        private readonly ITmdbApi _tmdbApi;

        public ApiDataSource()
        {
            _tmdbApi = new TmdbLibApi(TmdbApiHelper.TmdbApiKey);
        }

        public async Task<MoviesCredits> GetCreditsForMovieAsync(int movieId)
        {
            var movieCredits = await _tmdbApi.FetchMovieCreditsAsync(movieId);
            if (movieCredits != null)
            {
                var moviesCredits = new MoviesCredits()
                {
                    MovieId = movieId,
                    Credits = movieCredits
                };
                return moviesCredits;
            }
            return null;
        }

        public async Task<Movie> GetMovieAsync(string title, int releaseYear)
        {
            return await _tmdbApi.FetchMovieAsync(title, releaseYear);
        }

        public async Task<PersonsMovieCredits> GetMovieCreditsForPersonAsync(int personId)
        {
            var movieCredits = await _tmdbApi.FetchMovieCreditsForPerson(personId);
            if (movieCredits != null)
            {
                var personsMovieCredits = new PersonsMovieCredits()
                {
                    PersonId = personId,
                    MovieCredits = movieCredits
                };
                return personsMovieCredits;
            }

            return null;
        }
    }
}
