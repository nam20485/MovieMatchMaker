namespace MovieMatchMakerLibTests
{
    public class JsonFileCacheTests
    {      
        [Fact]
        public void Test_Load()
        {
            var dataCache = Utils.LoadJsonFileCache();
            dataCache.Should().NotBeNull();

            dataCache.Movies.Should().HaveCount(533);
            dataCache.MoviesCreditsById.Should().HaveCount(533);
            dataCache.PersonsMovieCreditsById.Should().HaveCount(23533);           
        }

        [Fact]
        public async void Test_GetMovieAsync_DarkCity_1998()
        {
            var dataCache = Utils.LoadJsonFileCache();
            dataCache.Should().NotBeNull();
           
            var movie = await dataCache.GetMovieAsync(Constants.Strings.DarkCityTitle, Constants.Numbers.DarkCityReleaseYear);
            movie.Should().NotBeNull();
            movie.MovieId.Should().Be(Constants.Numbers.DarkCityMovieId);
            movie.Title.Should().Be(Constants.Strings.DarkCityTitle);
            movie.ReleaseYear.Should().Be(Constants.Numbers.DarkCityReleaseYear);           
        }

        [Fact]
        public void Test_GetMovie_ById_DarkCity_1998()
        {
            var dataCache = Utils.LoadJsonFileCache();
            dataCache.Should().NotBeNull();

            var movieById = dataCache.GetMovie(Constants.Numbers.DarkCityMovieId);
            movieById.Should().NotBeNull();
            movieById.MovieId.Should().Be(Constants.Numbers.DarkCityMovieId);
            movieById.Title.Should().Be(Constants.Strings.DarkCityTitle);
            movieById.ReleaseYear.Should().Be(Constants.Numbers.DarkCityReleaseYear);
        }
    }
}
