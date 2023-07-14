using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerLibTests
{
    public class Utils
    {
        public static MovieConnectionBuilder CreateMovieConnectionBuilder(bool loaded)
        {
            var dataCache = JsonFileCache.Load(MovieDataBuilderBase.FilePath);
            var connectionBuilder = new MovieConnectionBuilder(dataCache);
            if (loaded)
            {
                connectionBuilder.LoadMovieConnections();
            }
            return connectionBuilder;
        }

        public static MovieConnectionBuilder CreateMovieConnectionBuilder()
        {
            var dataCache = JsonFileCache.Load(MovieDataBuilderBase.FilePath);
            var connectionBuilder = new MovieConnectionBuilder(dataCache);
            return connectionBuilder;
        }
    }
}
