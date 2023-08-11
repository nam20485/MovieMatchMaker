using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLibTests
{
    public class MovieConnectionsControllerTests
    {
        private readonly List<IMovieConnectionListFilter> _filters = 
            new(
                new IMovieConnectionListFilter[]
                {
                     new SortFilter(),
                     new MaxMatchingTitleWordsFilter(),
                     new MinConnectedRolesCountFilter(),
                });

        [Fact]
        public void Test_GetMovieConnections_DefaultFiltersApplied()
        {
            var controller = Utils.CreateMovieConnectionsController(true);

            var allConnections = controller.GetAllMovieConnections();
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(39);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovie_DefaultFiltersApplied_DarkCity_1998()
        {
            var controller = Utils.CreateMovieConnectionsController(true);

            var darkCityConnections = controller.GetMovieConnectionsForMovie(new MovieIdentifier("Dark City", 1998));
            darkCityConnections.Value.Should().NotBeNull();
            darkCityConnections.Value.Should().NotBeNull();
            //darkCityConnections.Should().HaveCount(0);
        }

        [Fact]
        public void Test_GetAllMovieConnectionsFiltered_DefaultFiltersApplied__AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController(true);

            var allConnections = controller.FilterAllMovieConnections(_filters);
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(30);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovieFiltered_DefaultFiltersApplied__DarkCity_1998_AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController(true);

            var allConnections = controller.FilterMovieConnectionsForMovie(new MovieIdentifier("Dark City", 1998), _filters);
            allConnections.Value.Should().NotBeNull();
            allConnections.Value.Should().NotBeNull();
            //allConnections.Should().HaveCount(0);
        }

        [Fact]
        public void Test_GetMovieConnections()
        {
            var controller = Utils.CreateMovieConnectionsController(false);

            var allConnections = controller.GetAllMovieConnections();
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(11754);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovie_DarkCity_1998()
        {
            var controller = Utils.CreateMovieConnectionsController(false);

            var darkCityConnections = controller.GetMovieConnectionsForMovie(new MovieIdentifier("Dark City", 1998));
            darkCityConnections.Value.Should().NotBeNull();
            darkCityConnections.Value.Should().NotBeNull();            
            darkCityConnections.Value.Should().HaveCount(305);
        }

        [Fact]
        public void Test_GetAllMovieConnectionsFiltered_AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController(false);

            var allConnections = controller.FilterAllMovieConnections(_filters);
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(947);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovieFiltered_DarkCity_1998_AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController(false);

            var allConnections = controller.FilterMovieConnectionsForMovie(new MovieIdentifier("Dark City", 1998), _filters);
            allConnections.Value.Should().NotBeNull();
            allConnections.Value.Should().NotBeEmpty();
            allConnections.Value.Should().HaveCount(14);
        }
    }
}
