using System.Collections.Generic;

using MovieMatchMakerLib.Model;

namespace MovieMatchMakerApi.Controllers
{
    public abstract class MovieConnectionsGraph<TExport>
    {
        protected readonly MovieConnection.List _movieConnections;

        protected MovieConnectionsGraph(IEnumerable<MovieConnection> movieConnections)
        {
            _movieConnections = new (movieConnections);
        }

        public TExport Export()
        {
            return BuildGraph();
        }

        protected abstract TExport BuildGraph();
    }
}