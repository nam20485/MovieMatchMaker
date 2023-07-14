using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib.DataCache;
using MovieMatchMakerLib;

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
