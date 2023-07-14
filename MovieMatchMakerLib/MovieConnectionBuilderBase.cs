using System.IO;
using System;
using System.Threading.Tasks;

using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public abstract class MovieConnectionBuilderBase : IMovieConnectionBuilder
    {               
        public static string FilePath => Path.Combine(SystemFolders.AppDataPath, "movieconnections.json");

        public MovieConnection.List MovieConnections { get; }

        protected readonly IDataCache _dataCache;        

        protected MovieConnectionBuilderBase(IDataCache dataCache)
        {
            MovieConnections = new MovieConnection.List();
            _dataCache = dataCache;
        }

        public abstract Task FindMovieConnections();

        public void LoadMovieConnections(string path)
        {
            var loaded = MovieConnection.List.LoadFromFile(path);
            MovieConnections.AddRange(loaded);
        }

        public void SaveMovieConnections(string path)
        {
            MovieConnections.SaveToFile(path);
        }       

        protected void AddMovieConnection(string name, Movie sourceMovie, string sourceRole, Movie targetMovie, string targetRole)
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
                    if (!movieConnection.ConnectedRoles.Contains(connectedRole))
                    {
                        movieConnection.ConnectedRoles.Add(connectedRole);
                    }
                }
            }
        }

        protected async Task FindMovieConnectionsFromRole(Movie sourceMovie, Cast sourceRole)
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

        protected async Task FindMovieConnectionsFromRole(Movie sourceMovie, Crew sourceRole)
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

        protected MovieConnection GetMovieConnection(Movie sourceMovie, Movie targetMovie)
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
    }
}