using System.Linq;
using System.Threading.Tasks;

using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib.TmdbApi
{
    public class TmdbLibApi : ITmdbApi
    {
        private readonly TMDbClient _apiClient;

        public TmdbLibApi(string apiKey)
        {
            _apiClient = new TMDbClient(apiKey);
        }

        public async Task<Credits> FetchMovieCreditsAsync(string title, int releaseYear)
        {
            var movie = await FetchMovieAsync(title, releaseYear);
            if (movie != null)
            {
                var credits = await _apiClient.GetMovieCreditsAsync(movie.MovieId);
                if (credits != null)
                {
                    return credits;
                }
            }

            return null;
        }

        public async Task<Model.Movie> FetchMovieAsync(string title, int releaseYear)
        {
            Model.Movie movie = null;

            var searchResult = await _apiClient.SearchMovieAsync(title, primaryReleaseYear: releaseYear);
            if (searchResult.TotalResults == 1)
            {
                var result = searchResult.Results.First();
                var movieId = result.Id;
                var posterImagePath = result.PosterPath;
                movie = new Model.Movie(title, releaseYear, movieId, posterImagePath);
            }

            return movie;
        }

        public async Task<MovieCredits> FetchMovieCreditsForPerson(int personApiId)
        {
            return await _apiClient.GetPersonMovieCreditsAsync(personApiId);
        }

        public async Task<Credits> FetchMovieCreditsAsync(int movieApiId)
        {
            return await _apiClient.GetMovieCreditsAsync(movieApiId);
        }
    }
}
