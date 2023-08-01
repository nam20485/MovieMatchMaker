using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Utils;

using TMDbLib.Objects.TvShows;

namespace MovieMatchMakerLib
{
    public class ThreadedMovieDataBuilder : IMovieDataBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.LocalAppDataPath, "moviedata.json");

        private readonly IDataSource _dataSource;

        private readonly RequestProcessingLoopThread<MovieRequest> _movieRequestsLoopThread;
        
        private DateTime _started;
        private DateTime _stopped;
        private bool disposedValue;

        public TimeSpan RunTime => _stopped - _started;

        public double MoviesFetchPerSecond => CalculateRate(_dataSource.MoviesFetched, DateTime.UtcNow - _started);       
        public double MovieCreditsFetchPerSecond => CalculateRate(_dataSource.MovieCreditsFetched, DateTime.UtcNow - _started);
        public double PersonMovieCreditsFetchPerSecond => CalculateRate(_dataSource.PersonMoviesCreditsFetched, DateTime.UtcNow - _started);

        public int MovieCreditsFetched => _dataSource.MovieCreditsFetched;
        public int MoviesFetched => _dataSource.MoviesFetched;
        public int PersonMovieCreditsFetched => _dataSource.PersonMoviesCreditsFetched;

        public int TaskCount => _movieRequestsLoopThread.TaskCount;

        private static double CalculateRate(int count, TimeSpan interval)
        {
            if (interval.TotalSeconds > 0)
            {
                return count / interval.TotalSeconds;
            }
            return 0.0;
        }       

        public ThreadedMovieDataBuilder(IDataSource dataSource)
        {
            _dataSource = dataSource;

            _movieRequestsLoopThread = new(ProcessMovieRequestAsync, true);            
        }

        public async Task BuildFreshFromInitial(string title, int releaseYear, int degree)
        {
            //await Task.Run(() =>
            //{
                _movieRequestsLoopThread.AddRequest(new MovieRequest(title, releaseYear, degree));
                //await ProcessMovieRequestAsync(new MovieRequest(title, releaseYear, degree));
                Start();
            //});
        }

        public void ContinueFromExisting(int degree)
        {
            // determine from the cache a list of movies to start fetching
            // movies in PersonsMoviesCredits that don't have corresponding Movies in the cache???
        }

        public void Start()
        {            
            _started = DateTime.UtcNow;
            _movieRequestsLoopThread.StartProcessingRequests();         
        }

        public void Stop()
        {
            // TODO: dispose here?
            //_dataSource.Dispose();
            _movieRequestsLoopThread.StopProcessingRequests();
            _stopped = DateTime.UtcNow;
        }

        public void Wait()
        {
            _movieRequestsLoopThread.Wait();           
        }

        protected async Task ProcessMovieRequestAsync(MovieRequest movieRequest)
        {
            var movie = await _dataSource.GetMovieAsync(movieRequest.Title, movieRequest.ReleaseYear);
            if (movie is not null)
            {
                if (movie.Fetched || movieRequest.Degree > 0)
                {
                    // if Fetched, then it wasn't from the cache. Otherwise it -came from the cache and
                    // therefor it's already had its credits and its roles' credits fetched and filled out
                    await ProcessMovieCreditsRequestAsync(new MovieCreditsRequest(movie.MovieId, movieRequest.Degree));
                }
            }            
        }

        protected async Task ProcessMovieCreditsRequestAsync(MovieCreditsRequest request)
        {
            var movieCredits = await _dataSource.GetCreditsForMovieAsync(request.MovieId);            
            if (movieCredits is not null)
            {
                foreach (var castRole in movieCredits.Credits.Cast)
                {
                    await ProcessPersonCreditsRequestAsync(new PersonCreditsRequest(castRole.Id, request.Degree));
                }
                foreach (var crewRole in movieCredits.Credits.Crew)
                {
                    await ProcessPersonCreditsRequestAsync(new PersonCreditsRequest(crewRole.Id, request.Degree));
                }
            }
        }

        protected async Task ProcessPersonCreditsRequestAsync(PersonCreditsRequest request)
        {
            var personCredits = await _dataSource.GetMovieCreditsForPersonAsync(request.PersonId);
            if (personCredits is not null)
            {
                if (request.Degree > 0)
                {
                    foreach (var castRole in personCredits.MovieCredits.Cast)
                    {
                        if (castRole.ReleaseDate.HasValue)
                        {
                            _movieRequestsLoopThread.AddRequest(new MovieRequest(castRole.Title, castRole.ReleaseDate.Value.Year, request.Degree - 1));
                            //await ProcessMovieRequestAsync(new MovieRequest(castRole.Title, castRole.ReleaseDate.Value.Year, request.Degree - 1));
                        }
                    }
                    foreach (var crewRole in personCredits.MovieCredits.Crew)
                    {
                        if (crewRole.ReleaseDate.HasValue)
                        {
                            _movieRequestsLoopThread.AddRequest(new MovieRequest(crewRole.Title, crewRole.ReleaseDate.Value.Year, request.Degree - 1));
                            //await ProcessMovieRequestAsync(new MovieRequest(crewRole.Title, crewRole.ReleaseDate.Value.Year, request.Degree - 1));
                        }
                    }
                }
            }
        }

        protected readonly struct MovieRequest
        {
            public readonly string Title;
            public readonly int ReleaseYear;
            public readonly int Degree;

            public MovieRequest(string title, int releaseYear, int degree)
            {
                Title = title;
                ReleaseYear = releaseYear;
                Degree = degree;
            }
        }

        protected readonly struct MovieCreditsRequest
        {
            public readonly int MovieId;
            public readonly int Degree;

            public MovieCreditsRequest(int movieId, int degree)
            {
                MovieId = movieId;
                Degree = degree;
            }
        }

        protected readonly struct PersonCreditsRequest
        {
            public readonly int PersonId;
            public readonly int Degree;

            public PersonCreditsRequest(int personId, int degree)
            {
                PersonId = personId;
                Degree = degree;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _dataSource.Dispose();
                    _movieRequestsLoopThread.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ThreadedMovieDataBuilder()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
