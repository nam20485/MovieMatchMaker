using Microsoft.AspNetCore.Mvc;

using MovieMatchMakerApi.Services;

using MovieMatchMakerLib.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieMatchMakerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieConnectionController : ControllerBase
    {
        private readonly ILogger<MovieConnectionController> _logger;
        private readonly MovieConnectionBuilderService _connectionBuilderService;

        public MovieConnectionController(MovieConnectionBuilderService connectionBuilderService, ILogger<MovieConnectionController> logger)
        {
            _connectionBuilderService = connectionBuilderService;
            _logger = logger;   
        }

        //[HttpGet(Name = "movieconnections")]
        //public async Task<MovieConnection.List> GetMovieConnections()
        //{
        //    //return _connectionBuilderService.
        //}

        // get movie connections for a movie

        // get all movie connections

        // get filtered movie connections
    }
}
