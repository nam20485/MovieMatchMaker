using MovieMatchMakerLib.Data;

namespace MovieMatchMakerLibTests
{
    public class DataSourceTests
    {
        private static string FilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "moviematchmaker.json");

        [Fact]
        public async void Test_GetMovie_DarkCity_1998()
        {
            var dataCache = JsonFileCache.Load(FilePath);
            var dataSource = new CachedDataSource(dataCache, new ApiDataSource());

            const string title = "Dark City";
            const int releaseYear = 1998;

            var movie = await dataSource.GetMovieAsync(title, releaseYear);
            Assert.NotNull(movie);
            Assert.Equal(title, movie.Title);
            Assert.Equal(releaseYear, movie.ReleaseYear);
        }
    }
}
