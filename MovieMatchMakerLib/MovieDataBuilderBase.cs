using System.IO;
using System.Threading.Tasks;
using MovieMatchMakerLib.DataSource;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public abstract class MovieDataBuilderBase : IMovieDataBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.AppDataPath, "moviematchmaker.json");

        protected readonly IDataSource _dataSource;        

        protected MovieDataBuilderBase(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public abstract Task BuildFromInitial(string title, int releaseYear, int degree);

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

        protected async Task FindMoviesConnectedToMovie(string title, int releaseYear, int degree)
        {
            if (degree >= 0)
            {
                var sourceMovie = await _dataSource.GetMovieAsync(title, releaseYear);
                if (sourceMovie != null)
                {
                    // not from the cache (if so we already have its credits)
                    if (sourceMovie.Fetched)
                    {
                        await FindMoviesConnectedToMovie(sourceMovie.MovieId, degree);
                    }
                }
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
    }
}