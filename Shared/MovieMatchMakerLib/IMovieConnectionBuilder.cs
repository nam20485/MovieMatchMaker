using System;
using System.Threading.Tasks;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib
{
    public interface IMovieConnectionBuilder : IDisposable
    {
        MovieConnection.List MovieConnections { get; }
        int MovieConnectionsFound { get; }
        double MovieConnectionsFoundPerSecond { get; }
        TimeSpan RunningTime { get; }
        TimeSpan TotalRunTime { get; }

        int MovieCreditsCount { get; }
        int MoviesCount { get; }
        int PersonMovieCreditsCount { get; }

        Task FindMovieConnections();
        void LoadMovieConnections(string path);
        void LoadMovieConnections();
        void SaveMovieConnections(string path);
        void SaveMovieConnections();

        void Start();
        void Stop();
        void Wait();
    }
}