using System.Threading.Tasks;

using MovieMatchMakerLib.DataSource;

namespace MovieMatchMakerLib
{
    public class MovieNetworkDataBuilder : MovieDataBuilderBase
    {
        public MovieNetworkDataBuilder(IDataSource dataSource)
            : base(dataSource)
        {         
        }

        public override async Task BuildFromInitial(string title, int releaseYear, int degree)
        {
            await FindMoviesConnectedToMovie(title, releaseYear, degree);
        }
    }
}
