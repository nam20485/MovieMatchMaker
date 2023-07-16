using MovieMatchMakerLib;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerApi.Services
{
    public class MovieConnectionsService : IMovieConnectionsService
    {
        public MovieConnection.List MovieConnections { get; }

        private readonly ILogger<MovieConnectionsService> _logger;
        
        public MovieConnectionsService(ILogger<MovieConnectionsService> logger)
        {
            _logger = logger;

            MovieConnections = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
        }
    }
}
