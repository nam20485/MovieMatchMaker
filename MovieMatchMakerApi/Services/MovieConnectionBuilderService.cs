using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerApi.Services
{
    public class MovieConnectionBuilderService
    {
        private readonly MovieConnectionBuilder _connectionBuilder;
        private readonly ILogger<MovieConnectionBuilderService> _logger;

        public MovieConnectionBuilderService(ILogger<MovieConnectionBuilderService> logger)
        {
            _logger = logger;

            var dataCache = JsonFileCache.Load(MovieDataBuilderBase.FilePath);                        
            _connectionBuilder = new MovieConnectionBuilder(dataCache);
        }

    }
}
