using MovieMatchMakerLib.TmdbApi;

namespace MovieMatchMakerLibTests
{
    public class TmdLibApiTests
    {
        [Fact]
        public void Test_GetTmdbApiKey()
        {
            var apiKey = TmdbApi.TmdbApiKey;
            Assert.NotNull(apiKey);
            Assert.NotEmpty(apiKey);            
        }

        [Fact]
        public async void Test_FetchMovieData_Exists()
        {
            var apiKey = TmdbApi.TmdbApiKey;
            Assert.NotNull(apiKey);
            Assert.NotEmpty(apiKey);

            ITmdbApi api = new TmdbLibApi(apiKey);
            var movie = await api.FetchMovieAsync("Dark City", 1998);
            Assert.NotNull(movie);            
        }

        [Fact]
        public async void Test_FetchMovieData_DoesNotExist()
        {
            var apiKey = TmdbApi.TmdbApiKey;
            Assert.NotNull(apiKey);
            Assert.NotEmpty(apiKey);

            ITmdbApi api = new TmdbLibApi(apiKey);
            var movie = await api.FetchMovieAsync("asdasd", 1678);
            Assert.Null(movie);            
        }
    }
}