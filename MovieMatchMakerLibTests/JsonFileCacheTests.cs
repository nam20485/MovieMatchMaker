namespace MovieMatchMakerLibTests
{
    public class JsonFileCacheTests
    {
        private const string title = "Dark City";
        private const int releaseYear = 1998;
        private const int darkCityMovieId = 2666;

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
           
            var movie = await dataCache.GetMovieAsync(title, releaseYear);
            movie.Should().NotBeNull();
            movie.MovieId.Should().Be(darkCityMovieId);
            movie.Title.Should().Be(title);
            movie.ReleaseYear.Should().Be(releaseYear);           
        }

        [Fact]
        public void Test_GetMovie_ById_DarkCity_1998()
        {           
            var dataCache = Utils.LoadJsonFileCache();
            dataCache.Should().NotBeNull();

            var movieById = dataCache.GetMovie(darkCityMovieId);
            movieById.Should().NotBeNull();
            movieById.MovieId.Should().Be(darkCityMovieId);
            movieById.Title.Should().Be(title);
            movieById.ReleaseYear.Should().Be(releaseYear);
        }
    }
}
