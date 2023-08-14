using Rubjerg.Graphviz;

namespace MovieMatchMakerLib.Model
{
    public class GraphvizMovieConnectionsCluster
    {
        private const string MovieIdAttributeName = "MovieId";
        private readonly RootGraph _rootGraph;
        private readonly MovieConnection.List _movieConnections;

        public GraphvizMovieConnectionsCluster(MovieConnection.List movieConnections)
        {
            _movieConnections = movieConnections;
            _rootGraph = RootGraph.CreateNew("Movie Connections Cluster", GraphType.Undirected);
        }

        public void BuildCluster()
        {
            foreach (var movieConnection in _movieConnections)
            {
                var sourceNode = _rootGraph.GetOrAddNode(movieConnection.SourceMovie.DisplayId);
                //sourceNode.SetAttribute(MovieIdAttributeName, movieConnection.SourceMovie.MovieId.ToString());

                var targetNode = _rootGraph.GetOrAddNode(movieConnection.TargetMovie.DisplayId);
                //targetNode.SetAttribute(MovieIdAttributeName, movieConnection.SourceMovie.MovieId.ToString());

                var edge = _rootGraph.GetOrAddEdge(targetNode, sourceNode, $"{movieConnection.SourceMovie.ApiId}-{movieConnection.TargetMovie.ApiId}");
                edge.SafeSetAttribute("label", movieConnection.ConnectedRoles.Count.ToString(), "default-value");                
            }
        }

        public void ExportToPngFile(string path)
        {
            _rootGraph.ComputeLayout();
            _rootGraph.ToPngFile(path);
            _rootGraph.FreeLayout();
        }
    }
}
