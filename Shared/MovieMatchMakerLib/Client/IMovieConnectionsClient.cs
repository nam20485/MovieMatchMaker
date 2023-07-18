using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Client
{
    public interface IMovieConnectionsClient
    {
        Task<MovieConnection.List> GetAllMovieConnections();
        Task<MovieConnection.List> GetMovieConnectionsForMovie(string title, int releaseYear);

        Task<MovieConnection.List> FilterAllMovieConnections(IEnumerable<IMovieConnectionListFilter> filters);

        Task<MovieConnection.List> FilterMovieConnectionsForMovie(string title, int releaseYear, IEnumerable<IMovieConnectionListFilter> filters);

    }
}
