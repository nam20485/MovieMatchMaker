using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public class MovieDataBuilder : IMovieDataBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.LocalAppDataPath, "moviematchmaker.json");

        protected readonly IDataSource _dataSource;
        private readonly bool _threaded;

        public MovieDataBuilder(IDataSource dataSource, bool threaded)
        {
            _dataSource = dataSource;
            _threaded = threaded;
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

        protected struct FindMoviesConnectedToMovieArgs
        {
            public string title;
            public int releaseYear;
            public int degree;
        }

        protected async void InvokeFindMovieConnectedToMovies(FindMoviesConnectedToMovieArgs args)
        {
            await FindMoviesConnectedToMovie(args.title, args.releaseYear, args.degree);
        }

        protected bool QueueFindMoviesConnectedToMovieThread(string title, int releaseYear, int degree)
        {
            const bool preferLocal = true;
            return ThreadPool.QueueUserWorkItem(InvokeFindMovieConnectedToMovies,
                                                new FindMoviesConnectedToMovieArgs
                                                {
                                                    title = title,
                                                    releaseYear = releaseYear,
                                                    degree = degree
                                                },
                                                preferLocal);
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
                    if (!_threaded)
                    {
                        await FindMoviesConnectedToMovie(role.Title, role.ReleaseDate.Value.Year, degree - 1);
                    }
                    else
                    {
                        QueueFindMoviesConnectedToMovieThread(role.Title, role.ReleaseDate.Value.Year, degree - 1);
                    }
                }
            }
            foreach (var role in personsMovieCredits.MovieCredits.Crew)
            {
                if (role.ReleaseDate.HasValue)
                {
                    if (!_threaded)
                    {
                        await FindMoviesConnectedToMovie(role.Title, role.ReleaseDate.Value.Year, degree - 1);
                    }
                    else
                    {
                        QueueFindMoviesConnectedToMovieThread(role.Title, role.ReleaseDate.Value.Year, degree - 1);
                    }
                }
            }
        }

        public async Task BuildFromInitial(string title, int releaseYear, int degree)
        {
            if (!_threaded)
            {
                await FindMoviesConnectedToMovie(title, releaseYear, degree);
            }
            else
            {
                QueueFindMoviesConnectedToMovieThread(title, releaseYear, degree);
            }
        }
    }
}
