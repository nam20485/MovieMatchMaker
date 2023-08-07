using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;

using TMDbLib.Client;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

using TvShowCredits = TMDbLib.Objects.TvShows.Credits;


namespace MovieMatchMakerLib.TmdbApi
{
    public class TmdbLibApi : ITmdbApi
    {
        private const int MaxRetryCount = 5;
        private const int InitialTimeoutDelayMs = 25;

        private readonly TMDbClient _apiClient;

        private bool disposedValue;
     
        public TmdbLibApi(string apiKey)
        {
            _apiClient = new TMDbClient(apiKey);
            //_apiClient.MaxRetryCount = 4;
            //_apiClient.GetConfigAsync().Wait();
        }       

        public async Task<Model.Movie> FetchMovieAsync(string title, int releaseYear)
        {
            Model.Movie movie = null;

            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ <= MaxRetryCount)
            {
                try
                {                    
                    var searchResult = await _apiClient.SearchMovieAsync(title, year: releaseYear);
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
                    Thread.Sleep(timeoutDelay *= 2);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                }
            }

            return movie;
        }

        public async Task<TvShow> FetchTvShowAsync(string name)
        {
            TvShow tvShow = null;

            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ <= MaxRetryCount)
            {
                try
                {
                    var searchResult = await _apiClient.SearchTvShowAsync(name);
                    // handle when multiple results are returned b/c they have the title as a keyword
                    var tvShowResult = searchResult.Results.FirstOrDefault(r =>
                    {
                        return r.Name == name;
                    });

                    if (tvShowResult is not null)
                    {
                        var apiId = tvShowResult.Id;
                        var year = tvShowResult.FirstAirDate.HasValue ? tvShowResult.FirstAirDate.Value.Year : 0;
                        var posterImagePath = tvShowResult.PosterPath;
                        tvShow = new TvShow(name, year, apiId, posterImagePath);
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
                    Thread.Sleep(timeoutDelay *= 2);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                }
            }

            return tvShow;
        }

        //public async Task<Credits> FetchMovieCreditsAsync(string title, int releaseYear)
        //{
        //    var timeoutDelay = InitialTimeoutDelayMs;
        //    var retryCount = 0;
        //    while (retryCount++ <= MaxRetryCount)
        //    {
        //        try
        //        {
        //            var movie = await FetchMovieAsync(title, releaseYear);
        //            if (movie != null)
        //            {
        //                var credits = await FetchMovieCreditsAsync(movie.ApiId);
        //                if (credits != null)
        //                {                            
        //                    return credits;
        //                }
        //            }
        //        }
        //        catch (TaskCanceledException tce)
        //        {
        //            ErrorLog.Log(tce);
        //            if (tce.InnerException is TimeoutException)
        //            {
        //                // request timed out
        //            }
        //        }
        //        catch (ObjectDisposedException ode)
        //        {
        //            ErrorLog.Log(ode);
        //        }
        //        catch (RequestLimitExceededException rlee)
        //        {
        //            ErrorLog.Log(rlee);
        //            Thread.Sleep(timeoutDelay *= 2);
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLog.Log(ex);
        //        }
        //    }

        //    return null;
        //}

        public async Task<Credits> FetchMovieCreditsAsync(int movieApiId)
        {
            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ <= MaxRetryCount)
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
                    Thread.Sleep(timeoutDelay *= 2);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                }
            }
            return null;
        }

        public async Task<TvShowCredits> FetchTvShowCreditsAsync(int tvShowId)
        {
            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ <= MaxRetryCount)
            {
                try
                {
                    return await _apiClient.GetTvShowCreditsAsync(tvShowId);
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
                    Thread.Sleep(timeoutDelay *= 2);
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex);
                }
            }
            return null;
        }

        //public async Task<Credits> FetchTvShowCreditsAsync(string title)
        //{
        //    var timeoutDelay = InitialTimeoutDelayMs;
        //    var retryCount = 0;
        //    while (retryCount++ <= MaxRetryCount)
        //    {
        //        try
        //        {
        //            var tvShow = await FetchTvShowAsync(title);
        //            if (tvShow != null)
        //            {
        //                var credits = await FetchMovieCreditsAsync(tvShow.ApiId);
        //                if (credits != null)
        //                {
        //                    return credits;
        //                }
        //            }
        //        }
        //        catch (TaskCanceledException tce)
        //        {
        //            ErrorLog.Log(tce);
        //            if (tce.InnerException is TimeoutException)
        //            {
        //                // request timed out
        //            }
        //        }
        //        catch (ObjectDisposedException ode)
        //        {
        //            ErrorLog.Log(ode);
        //        }
        //        catch (RequestLimitExceededException rlee)
        //        {
        //            ErrorLog.Log(rlee);
        //            Thread.Sleep(timeoutDelay *= 2);
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLog.Log(ex);
        //        }
        //    }

        //    return null;
        //}

        public async Task<MovieCredits> FetchMovieCreditsForPerson(int personApiId)
        {
            var timeoutDelay = InitialTimeoutDelayMs;
            var retryCount = 0;
            while (retryCount++ <= MaxRetryCount)
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
                    Thread.Sleep(timeoutDelay*=2);
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
            while (retryCount++ < MaxRetryCount)
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
                    Thread.Sleep(timeoutDelay *= 2);
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
