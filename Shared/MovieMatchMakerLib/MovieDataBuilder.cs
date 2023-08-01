using System;
using System.IO;
using System.Threading.Tasks;

using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public class MovieDataBuilder : IMovieDataBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.LocalAppDataPath, "moviematchmaker.json");

        private readonly IDataSource _dataSource;

        public double MoviesFetchPerSecond => MoviesFetched / (DateTime.UtcNow - _started).TotalSeconds;
        public double MovieCreditsFetchPerSecond => MovieCreditsFetched / (DateTime.UtcNow - _started).TotalSeconds;
        public double PersonMovieCreditsFetchPerSecond => PersonMovieCreditsFetched / (DateTime.UtcNow - _started).TotalSeconds;
        public double TotalFetchPerSecond => TotalFetched / (DateTime.UtcNow - _started).TotalSeconds;

        public int MovieCreditsFetched => _dataSource.MovieCreditsFetched;
        public int MoviesFetched => _dataSource.MoviesFetched;
        public int PersonMovieCreditsFetched => _dataSource.PersonMoviesCreditsFetched;
        public int TotalFetched => MoviesFetched + MovieCreditsFetched + PersonMovieCreditsFetched;

        private bool disposedValue;

        private DateTime _started;
        private DateTime _stopped;
        
        public TimeSpan TotalRunTime => _stopped - _started;
        public TimeSpan RunningTime => DateTime.UtcNow - _started;

        public int TaskCount => 0;      

        public MovieDataBuilder(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task BuildFreshFromInitial(string title, int releaseYear, int degree)
        {
            _started = DateTime.UtcNow;
            await FindMoviesConnectedToMovie(title, releaseYear, degree);            
        }

        protected async Task FindMoviesConnectedToMovie(string title, int releaseYear, int degree)
        {
            if (degree >= 0)
            {
                var sourceMovie = await _dataSource.GetMovieAsync(title, releaseYear);
                if (sourceMovie != null)
                {
                    // not from the cache (if so we already have its credits)
                    if (sourceMovie.Fetched || degree > 0)
                    {
                        await FindMoviesConnectedToMovie(sourceMovie.MovieId, degree);
                    }
                }
            }
        }

        protected struct FindMoviesConnectedToMovieArgs
        {
            public string title;
            public int releaseYear;
            public int degree;
        }       

        protected async Task FindMoviesConnectedToMovie(int movieId, int degree)
        {
            var moviesCredits = await _dataSource.GetCreditsForMovieAsync(movieId);
            foreach (var credit in moviesCredits.Credits.Cast)
            {
                await FindMoviesConnectedToPerson(credit.Id, degree);
            }
            foreach (var credit in moviesCredits.Credits.Crew)
            {
                await FindMoviesConnectedToPerson(credit.Id, degree);
            }
        }

        protected async Task FindMoviesConnectedToPerson(int personId, int degree)
        {
            var personsMovieCredits = await _dataSource.GetMovieCreditsForPersonAsync(personId);
            foreach (var role in personsMovieCredits.MovieCredits.Cast)
            {
                if (role.ReleaseDate.HasValue)
                {
                    await FindMoviesConnectedToMovie(role.Title, role.ReleaseDate.Value.Year, degree - 1);                  
                }
            }
            foreach (var role in personsMovieCredits.MovieCredits.Crew)
            {
                if (role.ReleaseDate.HasValue)
                {
                    await FindMoviesConnectedToMovie(role.Title, role.ReleaseDate.Value.Year, degree - 1);
                }
            }
        }

        public void ContinueFromExisting(int degree)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            // do nothing
            _started = DateTime.UtcNow;
        }

        public void Stop()
        {
            // do nothing
            Wait();
            _stopped = DateTime.UtcNow;
        }

        public void Wait()
        {
            // do nothing
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _dataSource.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MovieDataBuilder()
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

        //protected async void InvokeFindMovieConnectedToMovies(FindMoviesConnectedToMovieArgs args)
        //{
        //    await FindMoviesConnectedToMovie(args.title, args.releaseYear, args.degree);
        //}

        //protected bool QueueFindMoviesConnectedToMovieThread(string title, int releaseYear, int degree)
        //{
        //    const bool preferLocal = true;
        //    return ThreadPool.QueueUserWorkItem(InvokeFindMovieConnectedToMovies,
        //                                        new FindMoviesConnectedToMovieArgs
        //                                        {
        //                                            title = title,
        //                                            releaseYear = releaseYear,
        //                                            degree = degree
        //                                        },
        //                                        preferLocal);
        //}
    }
}
