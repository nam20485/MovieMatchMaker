using MovieMatchMakerLib;
using FluentAssertions;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerLibTests
{
    public class JsonFileCacheTests
    {      
        [Fact]
        public void Test_Load()
        {
            var dataCache = Utils.LoadJsonFileCache();
            dataCache.Should().NotBeNull();

            dataCache.Movies.Count.Should().Be(533);
            dataCache.MoviesCreditsById.Count.Should().Be(533);
            dataCache.PersonsMovieCreditsById.Count.Should().Be(23533);
           
        }

        [Fact]
        public async void Test_GetMovieAsync_DarkCity_1998()
        {
            const string title = "Dark City";
            const int releaseYear = 1998;

            var dataCache = Utils.LoadJsonFileCache();
            dataCache.Should().NotBeNull();
           
            var movie = await dataCache.GetMovieAsync(title, releaseYear);
            movie.Should().NotBeNull();
            movie.Title.Should().Be(title);
            movie.ReleaseYear.Should().Be(releaseYear);           
        }

        [Fact]
        public void Test_GetMovie_ById_DarkCity_1998()
        {
            const string title = "Dark City";
            const int releaseYear = 1998;
            const int darkCityMovieId = 2666;

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
