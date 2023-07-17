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

        private readonly IMovieConnectionsService _connectionsService;

        public MovieConnectionsController(ILogger<MovieConnectionsController> logger,
                                          IMovieConnectionsService connectionsService)
        {
            _logger = logger;

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
            return FindForMovie(title, releaseYear);
        }        

        // get filtered movie connections
        [HttpPost("movieconnections/filter")]
        public IEnumerable<MovieConnection> GetAllMovieConnectionsFiltered([FromBody] List<IMovieConnectionListFilter> filters)
        {
            return Filter(_connectionsService.MovieConnections, filters);            
        }      

        // get movie connections for a movie and then filter
        [HttpPost("movieconnections/filter/{title}/{releaseYear}")]
        public IEnumerable<MovieConnection> GetMovieConnectionsForMovieFiltered([FromRoute] string title, [FromRoute] int releaseYear, [FromBody] List<IMovieConnectionListFilter> filters)
        {            
            return Filter(FindForMovie(title, releaseYear), filters);
        }

        private MovieConnection.List FindForMovie(string title, int releaseYear)
        {
            return _connectionsService.MovieConnections.FindForMovie(title, releaseYear);
        }

        private static MovieConnection.List Filter(MovieConnection.List list, List<IMovieConnectionListFilter> filters)
        {
            return list.Filter(filters);
        }
    }
}
