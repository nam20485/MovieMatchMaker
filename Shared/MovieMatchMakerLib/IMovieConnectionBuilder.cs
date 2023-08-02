using System;
using System.Threading.Tasks;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib
{
    public interface IMovieConnectionBuilder : IDisposable
    {
        MovieConnection.List MovieConnections { get; }

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