using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerApi.Services
{
    public class MovieDataBuilderService : IMovieDataBuilderService
    {
        public MovieDataBuilder MovieDataBuilder { get; }

        private readonly ILogger<MovieDataBuilderService> _logger;

        public MovieDataBuilderService(ILogger<MovieDataBuilderService> logger)
        {
            _logger = logger;
            
            var cachedDataSource = CachedDataSource.CreateWithJsonFileCacheAndApiDataSource(MovieDataBuilder.FilePath, true);
            MovieDataBuilder = new MovieDataBuilder(cachedDataSource);
        }
    }
}
