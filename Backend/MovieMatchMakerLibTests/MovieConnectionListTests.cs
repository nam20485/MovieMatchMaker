using MovieMatchMakerLib;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLibTests
{
    public class MovieConnectionListTests
    {
        [Fact]
        public void Test_FilePath()
        {
            MovieConnectionBuilderBase.FilePath.Should().NotBeNull();
            MovieConnectionBuilderBase.FilePath.Should().NotBeEmpty();
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
        }

        [Fact]
        public void Test_LoadFromFile()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);
        }

        [Fact]
        public void Test_SaveToFile()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            const string filePath = "movieconnections_test_savetofile.json";
            loaded.SaveToFile(filePath);
            File.Exists(filePath).Should().BeTrue();
        }

        [Fact]
        public void Test_LoadSaveToFileRoundTrip()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            const string filePath = "movieconnections_test_savetofile.json";
            loaded.SaveToFile(filePath);
            File.Exists(filePath).Should().BeTrue();

            var loadedAgain = MovieConnection.List.LoadFromFile(filePath);
            loadedAgain.Should().NotBeNull();
            loadedAgain.Should().NotBeEmpty();
            loadedAgain.Should().HaveCount(loaded.Count);
            loadedAgain.Should().BeEquivalentTo(loaded);
        }

        [Fact]
        public void Test_FindForMovie()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            var movieConnectionsForDarkCity = loaded.FindForMovie(Constants.Strings.DarkCityTitle, Constants.Ints.DarkCityReleaseYear);
            movieConnectionsForDarkCity.Should().NotBeNull();
            movieConnectionsForDarkCity.Should().NotBeEmpty();
            movieConnectionsForDarkCity.Should().HaveCount(532);
        }

        [Fact]
        public void Test_Movies_All()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            var movies = loaded.Movies;
            movies.Should().NotBeNull();
            movies.Should().NotBeEmpty();
            movies.Should().HaveCount(533);
        }

        [Fact]
        public void Test_Movies_DarkCity_1998()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            var movieConnectionsForDarkCity = loaded.FindForMovie(Constants.Strings.DarkCityTitle, Constants.Ints.DarkCityReleaseYear);
            movieConnectionsForDarkCity.Should().NotBeNull();
            movieConnectionsForDarkCity.Should().NotBeEmpty();
            movieConnectionsForDarkCity.Should().HaveCount(532);

            var movies = movieConnectionsForDarkCity.Movies;
            movies.Should().NotBeNull();
            movies.Should().NotBeEmpty();
            movies.Should().HaveCount(533);
        }
    }
}
