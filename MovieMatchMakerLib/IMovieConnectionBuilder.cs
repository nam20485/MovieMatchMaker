using System.Threading.Tasks;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib
{
    public interface IMovieConnectionBuilder
    {
        MovieConnection.List MovieConnections { get; }

        Task FindMovieConnections();
        void LoadMovieConnections(string path);
        void SaveMovieConnections(string path);
    }
}