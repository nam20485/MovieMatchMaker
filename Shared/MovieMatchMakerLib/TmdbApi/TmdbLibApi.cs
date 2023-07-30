using System;
using System.Linq;
using System.Threading.Tasks;

using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;

namespace MovieMatchMakerLib.TmdbApi
{
    public class TmdbLibApi : ITmdbApi
    {
        private readonly TMDbClient _apiClient;
        private const int RetryCount = 5;

        public TmdbLibApi(string apiKey)
        {
            _apiClient = new TMDbClient(apiKey);
        }

        public async Task<Credits> FetchMovieCreditsAsync(string title, int releaseYear)
        {
            var retryCount = RetryCount;
            while (retryCount-- > 0)
            {
                try
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return null;
        }

        public async Task<Model.Movie> FetchMovieAsync(string title, int releaseYear)
        {
            Model.Movie movie = null;

            var retryCount = RetryCount;
            while (retryCount-- > 0)
            {
                try
                {
                    var searchResult = await _apiClient.SearchMovieAsync(title, primaryReleaseYear: releaseYear);
                    // handle when multiple results are returned b/c they have the title as a keyword
                    var movieResult = searchResult.Results.FirstOrDefault(r => r.Title == title && r.ReleaseDate.HasValue && r.ReleaseDate.Value.Year == releaseYear);

                    //SearchMovie result = null;
                    //if (searchResult.TotalResults == 1)
                    //{
                    //    result = searchResult.Results.First();
                    //}
                    //else if (searchResult.TotalResults > 1)
                    //{
                    //    result = searchResult.Results.FirstOrDefault(r => r.Title == title && r.ReleaseDate.HasValue && r.ReleaseDate.Value.Year == releaseYear);
                    //}

                    if (movieResult is not null)
                    {
                        var movieId = movieResult.Id;
                        var posterImagePath = movieResult.PosterPath;
                        movie = new Model.Movie(title, releaseYear, movieId, posterImagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return movie;
        }

        public async Task<MovieCredits> FetchMovieCreditsForPerson(int personApiId)
        {
            var retryCount = RetryCount;
            while (retryCount-- > 0)
            {
                try
                {
                    return await _apiClient.GetPersonMovieCreditsAsync(personApiId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return null;
        }

        public async Task<Credits> FetchMovieCreditsAsync(int movieApiId)
        {
            var retryCount = RetryCount;
            while (retryCount-- > 0)
            {
                try
                {
                    return await _apiClient.GetMovieCreditsAsync(movieApiId);                 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return null;
        }

        public async Task<ProfileImages> FetchImageDataForPerson(int personId)
        {
            var retryCount = RetryCount;
            while (retryCount-- > 0)
            {
                try
                {
                    return await _apiClient.GetPersonImagesAsync(personId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return null;
        }
    }
}
