using System;
using System.Threading.Tasks;

namespace MovieMatchMakerLib
{
    public interface IMovieDataBuilder  : IDisposable
    {
        double MovieCreditsFetchPerSecond { get; }
        double MoviesFetchPerSecond { get; }
        double PersonMovieCreditsFetchPerSecond { get; }

        int MovieCreditsFetched { get; }
        int MoviesFetched { get; }
        int PersonMovieCreditsFetched { get; }

        Task BuildFreshFromInitial(string title, int releaseYear, int degree);
        void ContinueFromExisting(int degree);

        void Start();
        void Stop();
        void Wait();

        TimeSpan RunTime { get; }
        int TaskCount { get; }
    }
}