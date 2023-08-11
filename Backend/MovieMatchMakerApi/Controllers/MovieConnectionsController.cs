using Microsoft.AspNetCore.Mvc;

using MovieMatchMakerApi.Services;
using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Graph;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;
using Rubjerg.Graphviz;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieMatchMakerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieConnectionsController : ControllerBase
    {
        private readonly ILogger<MovieConnectionsController> _logger;

        private readonly IMovieConnectionsService _connectionsService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly bool _applyDefaultFilters; 

        public MovieConnectionsController(ILogger<MovieConnectionsController> logger,
                                          IMovieConnectionsService connectionsService,
                                          IWebHostEnvironment env,
                                          bool applyDefaultFilters = true)
        {
            _logger = logger;
            _applyDefaultFilters = applyDefaultFilters;
            _connectionsService = connectionsService;
            _webHostEnvironment = env;
        }

        // get all movie connections
        [HttpGet("movieconnections")]
        public IEnumerable<MovieConnection> GetAllMovieConnections()
        {
            return GetMovieConnections();
        }

        // get movie connections for a movie
        [HttpGet("movieconnections/{Title}/{ReleaseYear:int}")]
        public ActionResult<IEnumerable<MovieConnection>> GetMovieConnectionsForMovie([FromRoute] MovieIdentifier movieId)
        {
            if (ModelState.IsValid)
            {
                return FindForMovie(movieId.Title, movieId.ReleaseYear);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }        

        // get filtered movie connections
        [HttpPost("movieconnections/filter")]
        public IEnumerable<MovieConnection> FilterAllMovieConnections([FromBody] List<IMovieConnectionListFilter> filters)
        {
            return Filter(GetMovieConnections(), filters);            
        }      

        // get movie connections for a movie and then filter
        [HttpPost("movieconnections/filter/{Title}/{ReleaseYear:int}")]
        public ActionResult<IEnumerable<MovieConnection>> FilterMovieConnectionsForMovie([FromRoute] MovieIdentifier movieId, [FromBody] List<IMovieConnectionListFilter> filters)
        {
            if (ModelState.IsValid)
            {
                return Filter(FindForMovie(movieId.Title, movieId.ReleaseYear), filters);
            }
            else
            {
                return BadRequest(ModelState);
            }
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
        //[Consumes(typeof(MovieIdentifier), "image/png", "image/svg+xml")]      
        [SwaggerResponse((int) HttpStatusCode.OK, "Returns graph image of movie's connections", typeof(FileContentResult), "image/png", "image/svg+xml")]
        [ProducesResponseType(typeof(FileContentResult), (int) HttpStatusCode.OK, "image/png", "image/svg+xml")]        
        [HttpGet("movieconnections/graph/{Title}/{ReleaseYear:int}")]
        public IActionResult GetMovieConnectionsGraphForMovie([FromRoute] MovieIdentifier movieId)
        {
            if (ModelState.IsValid)
            {
                var connections = FindForMovie(movieId.Title, movieId.ReleaseYear);
                var graph = new MovieConnectionsGraph(connections);
                var exportPath = $"{movieId.Title}_{movieId.ReleaseYear}_connections.png";
                //var mapped = _webHostEnvironment.MapPath(exportPath);

                var svg = true;                     
                if (svg)
                {
                    graph.ExportToSvgFile(exportPath);
                }
                else
                {
                    graph.ExportToPngFile(exportPath);
                }
                var bytes = System.IO.File.ReadAllBytes(exportPath);                
                return File(bytes, svg? "image/svg+xml" : "image/png");                
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
     
        private MovieConnection.List GetMovieConnections()
        {
            var movieConnections = _connectionsService.MovieConnections;
            if (_applyDefaultFilters)
            {
                movieConnections = movieConnections.Filter(DefaultMovieConnectionListFilters.Filters);
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
