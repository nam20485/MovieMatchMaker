using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MovieMatchMakerApi.Controllers;
using MovieMatchMakerApi.Services;

namespace MovieMatchMakerLibTests
{
    public class MovieConnectionsControllerTests
    {
        [Fact]
        public void Test_GetMovieConnections()
        {            
            var service = new MovieConnectionsService(Utils.CreateLogger<MovieConnectionsService>());
            var controller = new MovieConnectionsController(Utils.CreateLogger<MovieConnectionsController>(), service);

            var allConnections = controller.GetAllMovieConnections();
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(17413);
        }        
    }
}
