using System.Threading.Tasks;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerLib
{
    public class MovieDataBuilder : MovieDataBuilderBase
    {
        public MovieDataBuilder(IDataSource dataSource)
            : base(dataSource)
        {         
        }

        public override async Task BuildFromInitial(string title, int releaseYear, int degree)
        {
            await FindMoviesConnectedToMovie(title, releaseYear, degree);
        }
    }
}
