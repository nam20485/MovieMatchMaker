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
    public class ThreadedMovieConnectionBuilder : IMovieConnectionBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.LocalAppDataPath, "movieconnections.json");

        public MovieConnection.List MovieConnections { get; }

        protected readonly IDataCache _dataCache;

        private readonly RequestProcessingLoopThread<Model.Movie> _findMovieConnectionsLoopThread;
        private bool disposedValue;

        public ThreadedMovieConnectionBuilder(IDataCache dataCache)
        {
            MovieConnections = new MovieConnection.List();
            _dataCache = dataCache;
            _findMovieConnectionsLoopThread = new RequestProcessingLoopThread<Model.Movie>(FindMovieConnectionsFor);
        }

        public void LoadMovieConnections(string path)
        {
            var loaded = MovieConnection.List.LoadFromFile(path);
            MovieConnections.AddRange(loaded);
            //MovieConnections = MovieConnection.List.LoadFromFile(path);
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

        public async Task FindMovieConnections()
        {
            await Task.Run(() =>
            {
                foreach (var sourceMovie in _dataCache.Movies)
                {
                    //await FindMovieConnectionsFor(sourceMovie);
                    _findMovieConnectionsLoopThread.AddRequest(sourceMovie);
                }
            });           
        }

        private async Task FindMovieConnectionsFor(Model.Movie sourceMovie)
        {
            var sourceMoviesCredits = await _dataCache.GetCreditsForMovieAsync(sourceMovie.MovieId);
            foreach (var sourceRole in sourceMoviesCredits.Credits.Cast)
            {
                await FindMovieConnectionsFor(sourceMovie, sourceRole);
            }
            foreach (var sourceRole in sourceMoviesCredits.Credits.Crew)
            {
                await FindMovieConnectionsFor(sourceMovie, sourceRole);
            }
        }

        private async Task FindMovieConnectionsFor(Model.Movie sourceMovie, Cast sourceRole)
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

        private async Task FindMovieConnectionsFor(Model.Movie sourceMovie, Crew sourceRole)
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

        private void AddMovieConnection(string name, Model.Movie sourceMovie, string sourceRole, Model.Movie targetMovie, string targetRole, int personId, string profileImagePath)
        {
            if (targetMovie != null)
            {                
                if (targetMovie != sourceMovie)
                {
                    lock (MovieConnections)
                    {
                        var movieConnection = MovieConnections.GetOrCreateMovieConnection(sourceMovie, targetMovie);
                        var connectedRole = new ConnectedRole
                        {
                            Name = new Name(name),
                            SourceJob = sourceRole,
                            TargetJob = targetRole,
                            PersonId = personId,
                            PersonPosterPath = profileImagePath
                        };
                        if (!movieConnection.ConnectedRoles.Contains(connectedRole))
                        {
                            movieConnection.ConnectedRoles.Add(connectedRole);
                        }
                    }
                }
            }
        }

        public void Start()
        {
            _findMovieConnectionsLoopThread.StartProcessingRequests();
        }

        public void Stop()
        {
           _findMovieConnectionsLoopThread.StopProcessingRequests();
        }

        public void Wait()
        {
            _findMovieConnectionsLoopThread.Wait();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Stop();
                    _dataCache.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ThreadedMovieConnectionBuilder()
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
