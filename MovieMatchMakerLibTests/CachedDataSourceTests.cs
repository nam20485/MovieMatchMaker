namespace MovieMatchMakerLibTests
{
    public class CachedDataSourceTests
    {
        private const string title = "Dark City";
        private const int releaseYear = 1998;
        private const int darkCityMovieId = 2666;

        [Fact]
        public async void Test_GetMovie_DarkCity_1998()
        {
            var dataSource = Utils.CreateCachedDataSource();            

            var movie = await dataSource.GetMovieAsync(title, releaseYear);
            Assert.NotNull(movie);
            movie.MovieId.Should().Be(darkCityMovieId);
            Assert.Equal(title, movie.Title);
            Assert.Equal(releaseYear, movie.ReleaseYear);
        }       
    }
}
