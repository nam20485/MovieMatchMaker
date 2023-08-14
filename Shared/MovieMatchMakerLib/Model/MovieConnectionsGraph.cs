using System;

using MovieMatchMakerLib.Model;

namespace MovieMatchMakerApi.Controllers
{
    public abstract class MovieConnectionsGraph<TExport>
    {
        protected readonly MovieConnection.List _movieConnections;

        protected MovieConnectionsGraph(MovieConnection.List movieConnections)
        {
            _movieConnections = movieConnections;
        }

        public TExport Export()
        {
            return BuildGraph();
        }

        protected abstract TExport BuildGraph();
    }
}