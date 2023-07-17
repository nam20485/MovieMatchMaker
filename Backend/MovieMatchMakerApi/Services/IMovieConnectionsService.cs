using MovieMatchMakerLib.Model;

namespace MovieMatchMakerApi.Services
{
    public interface IMovieConnectionsService
    {
        MovieConnection.List MovieConnections { get; }
    }
}
