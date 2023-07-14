using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib
{
    public class MovieConnectionBuilder
    {
        private readonly IDataCache _dataCache;
        public MovieConnection.List MovieConnections { get; }

        public MovieConnectionBuilder(IDataCache dataCache)
        {
            MovieConnections = new MovieConnection.List();
            _dataCache = dataCache;
        }

        public async Task FindMovieConnections()
        {
            foreach (var sourceMovie in _dataCache.Movies)
            {
                var sourceMoviesCredits = await _dataCache.GetCreditsForMovieAsync(sourceMovie.MovieId);
                foreach (var sourceRole in sourceMoviesCredits.Credits.Cast)
                {
                    await FindMovieConnectionsFromRole(sourceMovie, sourceRole);
                }
                foreach (var sourceRole in sourceMoviesCredits.Credits.Crew)
                {
                    await FindMovieConnectionsFromRole(sourceMovie, sourceRole);
                }
            }
        }

        private async Task FindMovieConnectionsFromRole(Movie sourceMovie, Crew sourceRole)
        {
            var personCredits = await _dataCache.GetMovieCreditsForPersonAsync(sourceRole.Id);
            if (personCredits != null)
            {
                foreach (var targetRole in personCredits.MovieCredits.Crew)
                {
                    if (targetRole.ReleaseDate.HasValue)
                    {
                        var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                        AddMovieConnection(sourceRole.Name, sourceMovie, sourceRole.Job, targetMovie, targetRole.Job);
                    }
                }
                foreach (var targetRole in personCredits.MovieCredits.Cast)
                {
                    if (targetRole.ReleaseDate.HasValue)
                    {
                        var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                        AddMovieConnection(sourceRole.Name, sourceMovie, sourceRole.Job, targetMovie, targetRole.Character);
                    }
                }
            }
        }

        private async Task FindMovieConnectionsFromRole(Movie sourceMovie, Cast sourceRole)
        {
            var personCredits = await _dataCache.GetMovieCreditsForPersonAsync(sourceRole.Id);
            if (personCredits != null)
            {
                foreach (var targetRole in personCredits.MovieCredits.Cast)
                {
                    if (targetRole.ReleaseDate.HasValue)
                    {
                        var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                        AddMovieConnection(sourceRole.Name, sourceMovie, sourceRole.Character, targetMovie, targetRole.Character);
                    }
                }
                foreach (var targetRole in personCredits.MovieCredits.Crew)
                {
                    if (targetRole.ReleaseDate.HasValue)
                    {
                        var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                        AddMovieConnection(sourceRole.Name, sourceMovie, sourceRole.Character, targetMovie, targetRole.Job);
                    }
                }
            }
        }

        private void AddMovieConnection(string name, Movie sourceMovie, string sourceRole, Movie targetMovie, string targetRole)
        {
            if (targetMovie != null)
            {
                if (targetMovie != sourceMovie)
                {
                    var movieConnection = GetMovieConnection(sourceMovie, targetMovie);
                    var connectedRole = new ConnectedRole
                    {
                        Name = new Name(name),
                        SourceJob = sourceRole,
                        TargetJob = targetRole
                    };                   
                    if (! movieConnection.ConnectedRoles.Contains(connectedRole))
                    {
                        movieConnection.ConnectedRoles.Add(connectedRole);
                    }
                }
            }
        }     

        private MovieConnection GetMovieConnection(Movie sourceMovie, Movie targetMovie)
        {
            var movieConnection = MovieConnections.Find(mc =>
            {
                return ((mc.SourceMovie == sourceMovie && mc.TargetMovie == targetMovie) ||
                        (mc.SourceMovie == targetMovie && mc.TargetMovie == sourceMovie));
            });

            if (movieConnection is null)
            {
                // not found, return an empty new one
                movieConnection = new MovieConnection(sourceMovie, targetMovie);
                MovieConnections.Add(movieConnection);
            }

            return movieConnection;
        }

        public void LoadMovieConnections(string path)
        {
            var loaded = MovieConnection.List.LoadFromFile(path);
            MovieConnections.AddRange(loaded);
        }

        public void SaveMovieConnections(string path)
        {
            MovieConnections.SaveToFile(path);
        }
    }
}
