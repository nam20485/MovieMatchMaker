using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib
{
    public class ThreadedMovieNetworkDataBuilder : MovieNetworkDataBuilderBase
    {
        public ThreadedMovieNetworkDataBuilder(IDataSource dataSource)
            : base(dataSource)
        {
        }

        public override Task BuildFromInitial(string title, int releaseYear, int degree)
        {
            throw new NotImplementedException();
        }
    }
}
