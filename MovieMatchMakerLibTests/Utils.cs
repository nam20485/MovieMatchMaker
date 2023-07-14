using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerLibTests
{
    public class Utils
    {
        public static JsonFileCache LoadJsonFileCache()
        {
            var dataCache = JsonFileCache.Load(MovieDataBuilderBase.FilePath);
            return dataCache;
        }

        public static MovieConnectionBuilder CreateMovieConnectionBuilder(bool loaded)
        {
            var dataCache = LoadJsonFileCache();
            var connectionBuilder = new MovieConnectionBuilder(dataCache);
            if (loaded)
            {
                connectionBuilder.LoadMovieConnections();
            }
            return connectionBuilder;
        }        

        public static CachedDataSource CreateCachedDataSource()
        {
            var dataCache = LoadJsonFileCache();
            var dataSource = new CachedDataSource(dataCache, new ApiDataSource());
            return dataSource;
        }       
    }
}
