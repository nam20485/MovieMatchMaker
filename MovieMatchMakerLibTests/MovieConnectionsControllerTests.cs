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
        public void Test_GetMovieConnections()
        {
            var controller = Utils.CreateMovieConnectionsController();

            var allConnections = controller.GetAllMovieConnections();
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(17413);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovie_DarkCity_1998()
        {
            var controller = Utils.CreateMovieConnectionsController();

            var darkCityConnections = controller.GetMovieConnectionsForMovie("Dark City", 1998);
            darkCityConnections.Should().NotBeNull();
            darkCityConnections.Should().NotBeEmpty();
            darkCityConnections.Should().HaveCount(532);
        }

        [Fact]
        public void Test_GetAllMovieConnectionsFiltered_AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController();

            var allConnections = controller.GetAllMovieConnectionsFiltered(_filters);
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(2462);
        }

        [Fact]
        public void Test_GetMovieConnectionsForMovieFiltered_DarkCity_1998_AllFilters()
        {
            var controller = Utils.CreateMovieConnectionsController();

            var allConnections = controller.GetMovieConnectionsForMovieFiltered("Dark City", 1998, _filters);
            allConnections.Should().NotBeNull();
            allConnections.Should().NotBeEmpty();
            allConnections.Should().HaveCount(81);
        }
    }
}
