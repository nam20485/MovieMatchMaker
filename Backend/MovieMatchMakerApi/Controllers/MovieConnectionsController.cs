using Microsoft.AspNetCore.Mvc;

using MovieMatchMakerApi.Services;
using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Graph;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieMatchMakerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieConnectionsController : ControllerBase
    {
        private readonly ILogger<MovieConnectionsController> _logger;

        private readonly IMovieConnectionsService _connectionsService;

        private readonly bool _applyDefaultFilters = true;

        private static readonly IMovieConnectionListFilter[] _defaultFilters = new IMovieConnectionListFilter[]
        {
            new MinConnectedRolesCountFilter(3)
        };       

        public MovieConnectionsController(ILogger<MovieConnectionsController> logger,
                                          IMovieConnectionsService connectionsService,
                                          bool applyDefaultFilters = true)
        {
            _logger = logger;

            _applyDefaultFilters = applyDefaultFilters;
            _connectionsService = connectionsService;
        }

        // get all movie connections
        [HttpGet("movieconnections")]
        public IEnumerable<MovieConnection> GetAllMovieConnections()
        {
            return GetMovieConnections();
        }

        // get movie connections for a movie
        [HttpGet("movieconnections/{title}/{releaseYear}")]
        public IEnumerable<MovieConnection> GetMovieConnectionsForMovie([FromRoute] string title, [FromRoute] int releaseYear)
        {
            return FindForMovie(title, releaseYear);
        }        

        // get filtered movie connections
        [HttpPost("movieconnections/filter")]
        public IEnumerable<MovieConnection> FilterAllMovieConnections([FromBody] List<IMovieConnectionListFilter> filters)
        {
            return Filter(GetMovieConnections(), filters);            
        }      

        // get movie connections for a movie and then filter
        [HttpPost("movieconnections/filter/{title}/{releaseYear}")]
        public IEnumerable<MovieConnection> FilterMovieConnectionsForMovie([FromRoute] string title, [FromRoute] int releaseYear, [FromBody] List<IMovieConnectionListFilter> filters)
        {            
            return Filter(FindForMovie(title, releaseYear), filters);
        }

        // get movie connection by source and target movie        
        [HttpGet("movieconnection/{sourceMovieTitle}/{sourceMovieReleaseYear}/{targetMovieTitle}/{targetMovieReleaseYear}")]
        public MovieConnection GetMovieConnection([FromRoute] string sourceMovieTitle, [FromRoute] int sourceMovieReleaseYear, [FromRoute] string targetMovieTitle, [FromRoute] int targetMovieReleaseYear)
        {
            return GetMovieConnections().FindConnection(sourceMovieTitle, sourceMovieReleaseYear, targetMovieTitle, targetMovieReleaseYear);
        }

        // get movie connection by id
        [HttpGet("movieconnection/{id}")]
        public MovieConnection GetMovieConnection([FromRoute] int id)
        {
            return GetMovieConnections().FindConnection(id);
        }

        // get movie connections for a movie
        [HttpGet("movieconnections/graph/{title}/{releaseYear}")]
        public IActionResult GetMovieConnectionsGraphForMovie([FromRoute] string title, [FromRoute] int releaseYear)
        {
            var connections = FindForMovie(title, releaseYear);
            var graph = new MovieConnectionsGraph(connections);
            var exportPath = $"{title}_{releaseYear}_connections.png";
            graph.ExportToSvgFile(exportPath);
            var bytes = System.IO.File.ReadAllBytes(exportPath);
            return File(bytes, "image/svg");
        }

        private MovieConnection.List GetMovieConnections()
        {
            var movieConnections = _connectionsService.MovieConnections;
            if (_applyDefaultFilters)
            {
                movieConnections = movieConnections.Filter(_defaultFilters);
            }
            return movieConnections;
        }

        private MovieConnection.List FindForMovie(string title, int releaseYear)
        {
            return GetMovieConnections().FindForMovie(title, releaseYear);
        }

        private static MovieConnection.List Filter(MovieConnection.List list, List<IMovieConnectionListFilter> filters)
        {
            return list.Filter(filters);
        }
    }
}
