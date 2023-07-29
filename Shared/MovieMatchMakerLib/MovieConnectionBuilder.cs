using System.IO;
using System.Threading.Tasks;

using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;

using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace MovieMatchMakerLib
{
    public class MovieConnectionBuilder : IMovieConnectionBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.LocalAppDataPath, "movieconnections.json");

        public MovieConnection.List MovieConnections { get; }

        protected readonly IDataCache _dataCache;

        public MovieConnectionBuilder(IDataCache dataCache)
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
                        PersonPosterPath = profileImagePath
                    };
                    if (!movieConnection.ConnectedRoles.Contains(connectedRole))
                    {
                        movieConnection.ConnectedRoles.Add(connectedRole);
                    }
                }
            }
        }

        protected async Task FindMovieConnectionsFromRole(Model.Movie sourceMovie, Cast sourceRole)
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

        protected async Task FindMovieConnectionsFromRole(Model.Movie sourceMovie, Crew sourceRole)
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
    }
}
