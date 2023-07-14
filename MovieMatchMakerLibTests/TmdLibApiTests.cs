using MovieMatchMakerLib;

using Xunit;
using FluentAssertions;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLibTests
{
    public class TmdLibApiTests
    {
        [Fact]
        public void Test_GetTmdbApiKey()
        {
            var apiKey = Auth.TmdbApiKey;
            Assert.NotNull(apiKey);
            Assert.NotEmpty(apiKey);            
        }

        [Fact]
        public async void Test_FetchMovieData_Exists()
        {
            var apiKey = Auth.TmdbApiKey;
            Assert.NotNull(apiKey);
            Assert.NotEmpty(apiKey);

            ITmdbApi api = new TmdbLibApi(apiKey);
            var movie = await api.FetchMovieAsync("Dark City", 1998);
            Assert.NotNull(movie);            
        }

        [Fact]
        public async void Test_FetchMovieData_DoesNotExist()
        {
            var apiKey = Auth.TmdbApiKey;
            Assert.NotNull(apiKey);
            Assert.NotEmpty(apiKey);

            ITmdbApi api = new TmdbLibApi(apiKey);
            var movie = await api.FetchMovieAsync("asdasd", 1678);
            Assert.Null(movie);            
        }
    }
}