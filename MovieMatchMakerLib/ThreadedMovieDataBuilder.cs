using System;
using System.Threading;
using System.Threading.Tasks;
using MovieMatchMakerLib.Data;

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


        // void WaitCallback(Object state)  
        // ThreadPool.QueueUserWorkItem();

        //protected override async Task FindMoviesConnectedToMovie(string title, int releaseYear, int degree)
        //{
            
        //}
    }
}
