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

        [Fact]
        public void Test_GetOrCreateMovieConnection()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            var created = new MovieConnection.List();
            foreach (var mc in loaded)
            {
                var doesntExist = created.FindConnection(mc.SourceMovie.Title, mc.SourceMovie.ReleaseYear, mc.TargetMovie.Title, mc.TargetMovie.ReleaseYear);
                doesntExist.Should().BeNull();
                var newMc = created.GetOrCreateMovieConnection(mc.SourceMovie, mc.TargetMovie);
                newMc.Should().NotBeNull();
                var existsNow = created.FindConnection(mc.SourceMovie.Title, mc.SourceMovie.ReleaseYear, mc.TargetMovie.Title, mc.TargetMovie.ReleaseYear);
                existsNow.Should().NotBeNull();
                newMc.ConnectedRoles.Should().BeEmpty();
            }

            created.Should().HaveCount(loaded.Count);
            loaded.Should().BeEquivalentTo(created);
        }

        [Fact]
        public void Test_FindMovieConnectionExact()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            foreach (var mc in loaded)
            {
                var found = loaded.FindConnectionExact(mc.TargetMovie.Title, mc.TargetMovie.ReleaseYear, mc.SourceMovie.Title, mc.SourceMovie.ReleaseYear);
                found.Should().BeNull();                
            }            
        }

        [Fact]
        public void Test_FindMovieConnection()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            foreach (var mc in loaded)
            {
                var found = loaded.FindConnection(mc.SourceMovie.Title, mc.SourceMovie.ReleaseYear, mc.TargetMovie.Title, mc.TargetMovie.ReleaseYear);
                found.Should().NotBeNull();
                
                var foundReversed = loaded.FindConnection(mc.TargetMovie.Title, mc.TargetMovie.ReleaseYear, mc.SourceMovie.Title, mc.SourceMovie.ReleaseYear);
                foundReversed.Should().NotBeNull();
                
                found.Should().BeEquivalentTo(foundReversed);

                var foundById = loaded.FindConnection(found.Id);
                foundById.Should().NotBeNull();
            }
        }
    }
}
