using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerApi.Services
{
    public class MovieConnectionBuilderService : IMovieConnectionBuilderService
    {
        public MovieConnectionBuilder MovieConnectionBuilder { get; }

        private readonly ILogger<MovieConnectionBuilderService> _logger;        

        public MovieConnectionBuilderService(ILogger<MovieConnectionBuilderService> logger)
        {
            _logger = logger;

            var dataCache = JsonFileCache.Load(MovieDataBuilder.FilePath);                        
            MovieConnectionBuilder = new MovieConnectionBuilder(dataCache);            
        }
    }
}
