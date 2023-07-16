using Microsoft.Extensions.Logging;

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
            var apiDataSource = new ApiDataSource();
            var dataSource = new CachedDataSource(dataCache, apiDataSource);
            return dataSource;
        }  
        
        public static MovieDataBuilder CreateMovieDataBuilder()
        {            
            var cachedDataSource = CreateCachedDataSource();
            var movieNetworkDataBuilder = new MovieDataBuilder(cachedDataSource);
            return movieNetworkDataBuilder;
        }

        public static ILogger<T> CreateLogger<T>()
        {
            return LoggerFactory.Create(configure =>
            {
                //
            }).CreateLogger<T>();
        }
    }
}
