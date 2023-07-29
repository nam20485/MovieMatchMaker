using System.Threading.Tasks;

using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Data
{
    public class CachedDataSource : IDataSource
    {
        private readonly IDataCache _dataCache;
        private readonly IDataSource _dataSource;

        public CachedDataSource(IDataCache dataCache, IDataSource dataSource)
        {
            _dataCache = dataCache;
            _dataSource = dataSource;
        }

        public static CachedDataSource CreateWithJsonFileCacheAndApiDataSource(string cacheFilePath, bool loadCache)
        {
            JsonFileCache dataCache;
            if (loadCache)
            {
                dataCache = JsonFileCache.Load(cacheFilePath);
            }
            else
            {
                dataCache = new JsonFileCache(cacheFilePath);
            }
            var apiDataSource = new ApiDataSource();
            return new CachedDataSource(dataCache, apiDataSource);
        }

        public async Task<Movie> GetMovieAsync(string title, int releaseYear)
        {
            var movie = await _dataCache.GetMovieAsync(title, releaseYear);
            if (movie is null)
            {
                movie = await _dataSource.GetMovieAsync(title, releaseYear);
                if (movie != null)
                {
                    // TODO: does this work? (won't movies be stored int he cache as Fetched = true?)
                    movie.Fetched = true;
                    await _dataCache.AddMovieAsync(movie);
                }
            }
            else
            {
                movie.Fetched = false;
            }

            return movie;
        }        

        public async Task<PersonsMovieCredits> GetMovieCreditsForPersonAsync(int personId)
        {
            var personsMovieCredits = await _dataCache.GetMovieCreditsForPersonAsync(personId);
            if (personsMovieCredits is null)
            {
                personsMovieCredits = await _dataSource.GetMovieCreditsForPersonAsync(personId);
                if (personsMovieCredits != null)
                {                    
                    // add poster path
                    await _dataCache.AddPersonsMovieCreditsAsync(personsMovieCredits);
                }
            }

            return personsMovieCredits;
        }

        public async Task<MoviesCredits> GetCreditsForMovieAsync(int movieId)
        {
            var moviesCredits = await _dataCache.GetCreditsForMovieAsync(movieId);
            if (moviesCredits is null)
            {
                moviesCredits = await _dataSource.GetCreditsForMovieAsync(movieId);
                if (moviesCredits != null)
                {
                    await _dataCache.AddCreditsForMovieAsync(moviesCredits);
                }
            }

            return moviesCredits;
        }
    }
}