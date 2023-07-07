using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib;

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
            var movie = await dataSource.GetMovieAsync("Dark City", 1998);
            Assert.NotNull(movie);
        }
    }
}
