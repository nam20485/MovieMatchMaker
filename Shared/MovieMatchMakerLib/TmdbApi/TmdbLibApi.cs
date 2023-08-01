using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MovieMatchMakerLib.Utils;

using TMDbLib.Client;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;


namespace MovieMatchMakerLib.TmdbApi
{
    public class TmdbLibApi : ITmdbApi
    {
        private const int RetryCount = 5;

        private readonly TMDbClient _apiClient;
        private bool disposedValue;
        private const int InitialTimeoutDelayMs = 100;        
     
        public TmdbLibApi(string apiKey)
        {
            _apiClient = new TMDbClient(apiKey);
        }       

        public async Task<Model.Movie> FetchMovieAsync(string title, int releaseYear)
        {
            Model.Movie movie = null;

            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ < RetryCount)
            {
                try
                {
                    var searchResult = await _apiClient.SearchMovieAsync(title, primaryReleaseYear: releaseYear);
                    // handle when multiple results are returned b/c they have the title as a keyword
                    var movieResult = searchResult.Results.FirstOrDefault(r =>
                    {                        
                        return r.Title == title && 
                               r.ReleaseDate.HasValue &&
                               r.ReleaseDate.Value.Year == releaseYear;
                    });

                    if (movieResult is not null)
                    {
                        var movieId = movieResult.Id;
                        var posterImagePath = movieResult.PosterPath;
                        movie = new Model.Movie(title, releaseYear, movieId, posterImagePath);
                    }
                }
                catch (TaskCanceledException tce)
                {
                    ErrorLog.Log(tce);
                    if (tce.InnerException is TimeoutException)
                    {
                        // request timed out
                    }
                }
                catch (ObjectDisposedException ode)
                {
                    ErrorLog.Log(ode);
                }
                catch (RequestLimitExceededException rlee)
                {
                    ErrorLog.Log(rlee);
                    Thread.Sleep(Convert.ToInt32(timeoutDelay * Math.Pow(2, retryCount)));
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                }
            }

            return movie;
        }

        public async Task<Credits> FetchMovieCreditsAsync(string title, int releaseYear)
        {
            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ < RetryCount)
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
                catch (TaskCanceledException tce)
                {
                    ErrorLog.Log(tce);
                    if (tce.InnerException is TimeoutException)
                    {
                        // request timed out
                    }
                }
                catch (ObjectDisposedException ode)
                {
                    ErrorLog.Log(ode);
                }
                catch (RequestLimitExceededException rlee)
                {
                    ErrorLog.Log(rlee);
                    Thread.Sleep(Convert.ToInt32(timeoutDelay * Math.Pow(2, retryCount)));
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                }
            }

            return null;
        }

        public async Task<Credits> FetchMovieCreditsAsync(int movieApiId)
        {
            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ < RetryCount)
            {
                try
                {
                    return await _apiClient.GetMovieCreditsAsync(movieApiId);
                }
                catch (TaskCanceledException tce)
                {
                    ErrorLog.Log(tce);
                    if (tce.InnerException is TimeoutException)
                    {
                        // request timed out
                    }
                }
                catch (ObjectDisposedException ode)
                {
                    ErrorLog.Log(ode);
                }
                catch (RequestLimitExceededException rlee)
                {
                    ErrorLog.Log(rlee);
                    Thread.Sleep(Convert.ToInt32(timeoutDelay * Math.Pow(2, retryCount)));
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                }
            }
            return null;
        }

        public async Task<MovieCredits> FetchMovieCreditsForPerson(int personApiId)
        {
            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ < RetryCount)
            {
                try
                {
                    return await _apiClient.GetPersonMovieCreditsAsync(personApiId);
                }
                catch (TaskCanceledException tce)
                {
                    ErrorLog.Log(tce);
                    if (tce.InnerException is TimeoutException)
                    {
                        // request timed out
                    }
                }
                catch (RequestLimitExceededException rlee)
                {
                    ErrorLog.Log(rlee);
                    Thread.Sleep(Convert.ToInt32(timeoutDelay * Math.Pow(2, retryCount)));
                }
                catch (ObjectDisposedException ode)
                {
                    ErrorLog.Log(ode);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                }
            }
            return null;
        }       

        public async Task<ProfileImages> FetchImageDataForPerson(int personId)
        {
            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ < RetryCount)
            {
                try
                {
                    return await _apiClient.GetPersonImagesAsync(personId);
                }
                catch (TaskCanceledException tce)
                {
                    ErrorLog.Log(tce);
                    if (tce.InnerException is TimeoutException)
                    {
                        // request timed out
                    }
                }
                catch (ObjectDisposedException ode)
                {
                    ErrorLog.Log(ode);
                }
                catch (RequestLimitExceededException rlee)
                {
                    ErrorLog.Log(rlee);
                    Thread.Sleep(Convert.ToInt32(timeoutDelay * Math.Pow(2, retryCount)));
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                }
            }
            return null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _apiClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }
       
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
