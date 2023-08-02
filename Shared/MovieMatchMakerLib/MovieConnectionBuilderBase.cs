using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;

using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace MovieMatchMakerLib
{
    public abstract class MovieConnectionBuilderBase : IMovieConnectionBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.LocalAppDataPath, "movieconnections.json");

        public MovieConnection.List MovieConnections { get; }

        public TimeSpan TotalRunTime => _stopped - _started;
        public TimeSpan RunningTime => DateTime.UtcNow - _started;
        public double MovieConnectionsFoundPerSecond => CalculateRate(MovieConnectionsFound, DateTime.UtcNow - _started);

        public int MovieConnectionsFound => MovieConnections.Count;
        public int MovieCreditsCount => _dataCache.MovieCreditsCount;
        public int MoviesCount => _dataCache.MovieCount;
        public int PersonMovieCreditsCount => _dataCache.PersonMoviesCreditsCount;

        protected readonly IDataCache _dataCache;

        private bool disposedValue;

        protected DateTime _started;
        protected DateTime _stopped;                

        protected MovieConnectionBuilderBase(IDataCache dataCache)
        {
            MovieConnections = new MovieConnection.List();
            _dataCache = dataCache;
        }

        public void LoadMovieConnections(string path)
        {
            var loaded = MovieConnection.List.LoadFromFile(path);
            MovieConnections.AddRange(loaded);
        }

        public void LoadMovieConnections()
        {
            LoadMovieConnections(FilePath);
        }

        public void SaveMovieConnections(string path)
        {
            MovieConnections.SaveToFile(path);
        }

        public void SaveMovieConnections()
        {
            SaveMovieConnections(FilePath);
        }

        protected void AddMovieConnection(string name, Model.Movie sourceMovie, string sourceRole, Model.Movie targetMovie, string targetRole, int personId, string profileImagePath)
        {
            if (targetMovie != null)
            {
                if (targetMovie != sourceMovie)
                {
                    var movieConnection = MovieConnections.GetOrCreateMovieConnection(sourceMovie, targetMovie);
                    var connectedRole = new ConnectedRole
                    {
                        Name = new Name(name),
                        SourceJob = sourceRole,
                        TargetJob = targetRole,
                        PersonId = personId,
                        PersonPosterPathSuffix = profileImagePath
                    };
                    if (!movieConnection.ConnectedRoles.Contains(connectedRole))
                    {
                        movieConnection.ConnectedRoles.Add(connectedRole);
                    }
                }
            }
        }

        protected async Task FindMovieConnectionsFor(Model.Movie sourceMovie, Cast sourceRole)
        {
            var personCredits = await _dataCache.GetMovieCreditsForPersonAsync(sourceRole.Id);
            if (personCredits != null)
            {
                foreach (var targetRole in personCredits.MovieCredits.Cast)
                {
                    if (targetRole.ReleaseDate.HasValue)
                    {
                        var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                        AddMovieConnection(sourceRole.Name, sourceMovie, sourceRole.Character, targetMovie, targetRole.Character, personCredits.PersonId, personCredits.ProfileImagePath);
                    }
                }
                foreach (var targetRole in personCredits.MovieCredits.Crew)
                {
                    if (targetRole.ReleaseDate.HasValue)
                    {
                        var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                        AddMovieConnection(sourceRole.Name, sourceMovie, sourceRole.Character, targetMovie, targetRole.Job, personCredits.PersonId, personCredits.ProfileImagePath);
                    }
                }
            }
        }

        protected async Task FindMovieConnectionsFor(Model.Movie sourceMovie, Crew sourceRole)
        {
            var personCredits = await _dataCache.GetMovieCreditsForPersonAsync(sourceRole.Id);
            if (personCredits != null)
            {
                foreach (var targetRole in personCredits.MovieCredits.Crew)
                {
                    if (targetRole.ReleaseDate.HasValue)
                    {
                        var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                        AddMovieConnection(sourceRole.Name, sourceMovie, sourceRole.Job, targetMovie, targetRole.Job, personCredits.PersonId, personCredits.ProfileImagePath);
                    }
                }
                foreach (var targetRole in personCredits.MovieCredits.Cast)
                {
                    if (targetRole.ReleaseDate.HasValue)
                    {
                        var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                        AddMovieConnection(sourceRole.Name, sourceMovie, sourceRole.Job, targetMovie, targetRole.Character, personCredits.PersonId, personCredits.ProfileImagePath);
                    }
                }
            }
        }

        protected async Task FindMovieConnectionsFor(Model.Movie sourceMovie)
        {
            var sourceMoviesCredits = await _dataCache.GetCreditsForMovieAsync(sourceMovie.MovieId);
            if (sourceMoviesCredits != null)
            {
                foreach (var sourceRole in sourceMoviesCredits.Credits.Cast)
                {
                    await FindMovieConnectionsFor(sourceMovie, sourceRole);
                }
                foreach (var sourceRole in sourceMoviesCredits.Credits.Crew)
                {
                    await FindMovieConnectionsFor(sourceMovie, sourceRole);
                }
            }
        }       

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _dataCache.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MovieConnectionBuilder()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }

        public abstract Task FindMovieConnections();

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        public virtual void Wait()
        {
        }

        private static double CalculateRate(int count, TimeSpan interval)
        {
            if (interval.TotalSeconds > 0)
            {
                return count / interval.TotalSeconds;
            }
            return 0.0;
        }
    }
}
