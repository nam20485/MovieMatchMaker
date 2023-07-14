using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMDbLib.Objects.Movies;

namespace MovieMatchMakerLib
{
    public class MovieNetworkDataBuilder : MovieNetworkDataBuilderBase
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
