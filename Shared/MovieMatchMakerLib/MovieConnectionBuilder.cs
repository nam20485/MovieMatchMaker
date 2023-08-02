using System.Threading.Tasks;

using MovieMatchMakerLib.Data;

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
