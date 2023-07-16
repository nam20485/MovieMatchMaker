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

        private readonly MovieConnectionBuilderService _connectionBuilderService;
        private readonly MovieConnectionsService _connectionsService;

        public MovieConnectionsController(ILogger<MovieConnectionsController> logger,
                                          MovieConnectionBuilderService connectionBuilderService,
                                          MovieConnectionsService connectionsService)
        {
            _logger = logger;

            _connectionBuilderService = connectionBuilderService;
            _connectionsService = connectionsService;             
        }

        // get all movie connections
        [HttpGet(Name = "movieconnections")]
        public IEnumerable<MovieConnection> GetMovieConnections()
        {
            return _connectionsService.MovieConnections;
        }

        // get movie connections for a movie
        [HttpGet("{title}/{releaseYear}", Name = "movie")]
        public ActionResult GetMovieConnectionForMovie([FromRoute] string title, [FromRoute] int releaseYear)
        {
            var movieConnection =  _connectionsService.MovieConnections.Find(mc =>
            {
                return (mc.SourceMovie.Title == title && mc.SourceMovie.ReleaseYear == releaseYear) ||
                       (mc.TargetMovie.Title == title && mc.TargetMovie.ReleaseYear == releaseYear);
            });  
            
            if (movieConnection != null)
            {
                return Ok(movieConnection); 
            }

            return NotFound();
        }


        // get filtered movie connections
        [HttpPost(Name = "filter")]
        public IEnumerable<MovieConnection> FilterMovieConnections([FromBody] List<IMovieConnectionListFilter> filters)
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
