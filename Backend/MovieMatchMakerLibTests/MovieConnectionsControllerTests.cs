using MovieMatchMakerLib.Filters;

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
            allConnections.Should().HaveCount(974);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovie_DefaultFiltersApplied_DarkCity_1998()
        {
            var controller = Utils.CreateMovieConnectionsController(true);

            var darkCityConnections = controller.GetMovieConnectionsForMovie("Dark City", 1998);
            darkCityConnections.Should().NotBeNull();
            darkCityConnections.Should().NotBeEmpty();
            darkCityConnections.Should().HaveCount(45);
        }

        [Fact]
        public void Test_GetAllMovieConnectionsFiltered_DefaultFiltersApplied__AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController(true);

            var allConnections = controller.FilterAllMovieConnections(_filters);
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(809);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovieFiltered_DefaultFiltersApplied__DarkCity_1998_AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController(true);

            var allConnections = controller.FilterMovieConnectionsForMovie("Dark City", 1998, _filters);
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(44);
        }

        [Fact]
        public void Test_GetMovieConnections()
        {
            var controller = Utils.CreateMovieConnectionsController(false);

            var allConnections = controller.GetAllMovieConnections();
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(17413);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovie_DarkCity_1998()
        {
            var controller = Utils.CreateMovieConnectionsController(false);

            var darkCityConnections = controller.GetMovieConnectionsForMovie("Dark City", 1998);
            darkCityConnections.Should().NotBeNull();
            darkCityConnections.Should().NotBeEmpty();
            darkCityConnections.Should().HaveCount(532);
        }

        [Fact]
        public void Test_GetAllMovieConnectionsFiltered_AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController(false);

            var allConnections = controller.FilterAllMovieConnections(_filters);
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(2462);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovieFiltered_DarkCity_1998_AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController(false);

            var allConnections = controller.FilterMovieConnectionsForMovie("Dark City", 1998, _filters);
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(81);
        }
    }
}
