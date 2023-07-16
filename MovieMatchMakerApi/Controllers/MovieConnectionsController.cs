using Microsoft.AspNetCore.Mvc;

using MovieMatchMakerApi.Services;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieMatchMakerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieConnectionsController : ControllerBase
    {
        private readonly ILogger<MovieConnectionsController> _logger;

        private readonly IMovieDataBuilderService _dataBuilderService;
        private readonly IMovieConnectionBuilderService _connectionBuilderService;
        private readonly IMovieConnectionsService _connectionsService;

        public MovieConnectionsController(ILogger<MovieConnectionsController> logger,
                                          IMovieDataBuilderService dataBuilderService,
                                          IMovieConnectionBuilderService connectionBuilderService,
                                          IMovieConnectionsService connectionsService)
        {
            _logger = logger;

            _dataBuilderService = dataBuilderService;
            _connectionBuilderService = connectionBuilderService;
            _connectionsService = connectionsService;             
        }

        // get all movie connections
        [HttpGet("movieconnections")]
        public IEnumerable<MovieConnection> GetAllMovieConnections()
        {
            return _connectionsService.MovieConnections;
        }

        // get movie connections for a movie
        [HttpGet("movieconnections/{title}/{releaseYear}")]
        public IEnumerable<MovieConnection> GetMovieConnectionsForMovie([FromRoute] string title, [FromRoute] int releaseYear)
        {
            var movieConnections =  _connectionsService.MovieConnections.FindAll(mc =>
            {
                return (mc.SourceMovie.Title == title && mc.SourceMovie.ReleaseYear == releaseYear) ||
                       (mc.TargetMovie.Title == title && mc.TargetMovie.ReleaseYear == releaseYear);
            });  
            
            return movieConnections;             
        }


        // get filtered movie connections
        [HttpPost("movieconnections/filter")]
        public IEnumerable<MovieConnection> GetFiltereredMovieConnections([FromBody] List<IMovieConnectionListFilter> filters)
        {
            var filtered = _connectionsService.MovieConnections;
            foreach (var filter in filters)
            {
                filtered = filter.Apply(filtered);
            }
            return filtered;
        }
    }
}
