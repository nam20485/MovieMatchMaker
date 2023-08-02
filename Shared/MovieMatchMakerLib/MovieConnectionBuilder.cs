using System.IO;
using System.Threading.Tasks;

using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;

using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace MovieMatchMakerLib
{
    public class MovieConnectionBuilder : MovieConnectionBuilderBase
    {        
        public MovieConnectionBuilder(IDataCache dataCache)
            : base(dataCache)
        {           
        }

        public override async Task FindMovieConnections()
        {
            foreach (var sourceMovie in _dataCache.Movies)
            {
                await FindMovieConnectionsFor(sourceMovie);
            }
        }        
    }
}
