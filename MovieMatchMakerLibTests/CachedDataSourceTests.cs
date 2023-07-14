using MovieMatchMakerLib.Data;

namespace MovieMatchMakerLibTests
{
    public class CachedDataSourceTests
    {        
        [Fact]
        public async void Test_GetMovie_DarkCity_1998()
        {
            var dataSource = Utils.CreateCachedDataSource();

            const string title = "Dark City";
            const int releaseYear = 1998;

            var movie = await dataSource.GetMovieAsync(title, releaseYear);
            Assert.NotNull(movie);
            Assert.Equal(title, movie.Title);
            Assert.Equal(releaseYear, movie.ReleaseYear);
        }       
    }
}
