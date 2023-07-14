using System;
using System.Threading.Tasks;

using MovieMatchMakerLib.DataSource;

namespace MovieMatchMakerLib
{
    public class ThreadedMovieDataBuilder : MovieDataBuilderBase
    {
        public ThreadedMovieDataBuilder(IDataSource dataSource)
            : base(dataSource)
        {
        }

        public override Task BuildFromInitial(string title, int releaseYear, int degree)
        {
            throw new NotImplementedException();
        }
    }
}
