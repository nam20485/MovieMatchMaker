using MovieMatchMakerLib.Filters;

namespace MovieMatchMakerLibTests
{
    public class FilterTests
    {
        [Fact]
        public void Test_MinConnectedRolesCountFilter_2()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new MinConnectedRolesCountFilter(2).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(2734);
        }

        [Fact]
        public void Test_MinConnectedRolesCountFilter_4()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new MinConnectedRolesCountFilter(4).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(533);
        }

        [Fact]
        public void Test_MinConnectedRolesCountFilter_8()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new MinConnectedRolesCountFilter(8).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(136);
        }

        [Fact]
        public void Test_MinConnectedRolesCountFilter_16()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new MinConnectedRolesCountFilter(16).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(38);
        }

        [Fact]
        public void Test_MinConnectedRolesCountFilter_32()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new MinConnectedRolesCountFilter(32).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(10);
        }

        [Fact]
        public void Test_MinConnectedRolesCountFilter_64()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new MinConnectedRolesCountFilter(64).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(2);
        }
       
        [Fact]
        public void Test_MaxMatchingTitleWordsFilter_0()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new MaxMatchingTitleWordsFilter(0).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(15760);            
        }

        [Fact]
        public void Test_MaxMatchingTitleWordsFilter_1()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new MaxMatchingTitleWordsFilter(1).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(17299);
        }

        [Fact]
        public void Test_MaxMatchingTitleWordsFilter_2()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new MaxMatchingTitleWordsFilter(2).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(17376);
        }

        [Fact]
        public void Test_SpecificMovieFilter_DarkCity_1998()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered = new SpecificMovieFilter("Dark City", 1998).Apply(connectionBuilder.MovieConnections);
            filtered.Should().HaveCount(532);
        }

        [Fact]
        public void Test_AllFilters_DefaultSettings()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered =
                new MaxMatchingTitleWordsFilter().Apply(
                    new MinConnectedRolesCountFilter().Apply(
                        new SortFilter().Apply(
                            connectionBuilder.MovieConnections)));
            filtered.Should().HaveCount(2462);
        }

        [Fact]
        public void Test_AllFiltersWithSpecificMovie_DefaultSettings()
        {
            var connectionBuilder = Utils.CreateMovieConnectionBuilder(true);
            connectionBuilder.MovieConnections.Should().HaveCount(17413);
            var filtered =
                new MaxMatchingTitleWordsFilter().Apply(
                    new MinConnectedRolesCountFilter().Apply(
                        new SpecificMovieFilter("Dark City", 1998).Apply(
                            new SortFilter().Apply(                            
                                connectionBuilder.MovieConnections))));
            filtered.Should().HaveCount(81);
        }
    }
}
