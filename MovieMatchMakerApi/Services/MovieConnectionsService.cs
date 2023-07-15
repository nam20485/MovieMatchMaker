using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerApi.Services
{
    public class MovieConnectionsService : IMovieConnectionsService
    {
        public MovieConnection.List MovieConnections { get; }

        private readonly ILogger<MovieConnectionBuilderService> _logger;
        
        public MovieConnectionsService(ILogger<MovieConnectionBuilderService> logger)
        {
            _logger = logger;

            MovieConnections = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
        }
    }
}
