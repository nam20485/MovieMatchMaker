using Rubjerg.Graphviz;


using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib.Graph
{
    public class MovieConnectionsGraph
    {
        private readonly MovieConnection.List _movieConnections;
        private RootGraph _rootGraph;

        public MovieConnectionsGraph(MovieConnection.List movieConnections)
        {
            _movieConnections = movieConnections;
            _rootGraph = BuildGraph();
        }

        private RootGraph BuildGraph()
        {                       
            var rootGraph = RootGraph.CreateNew("Movie Connections", GraphType.Undirected);

            foreach (var connection in _movieConnections)
            {                
                if (!connection.TargetMovie.DisplayId.ContainsNonAsciiChars() &&
                    !connection.SourceMovie.DisplayId.ContainsNonAsciiChars())
                {
                    var sourceMovieNode = rootGraph.GetOrAddNode(connection.SourceMovie.DisplayId);
                    var targetMovieNode = rootGraph.GetOrAddNode(connection.TargetMovie.DisplayId);

                    var edge = rootGraph.GetOrAddEdge(sourceMovieNode, targetMovieNode, connection.ConnectedRoles.Count.ToString());
                    edge.SafeSetAttribute("label", connection.ConnectedRoles.Count.ToString(), "default-value");
                }
            }

            TrimGraph(rootGraph);

            return rootGraph;
        }

        private void TrimGraph(RootGraph rootGraph)
        {
            foreach (var n in rootGraph.Nodes())
            {
                
            }
            // remove 'subgraphs' with less than n nodes
        }

        private int CountConnectedNodes(Node n)
        {
            return -1;
        }

        public void ExportToSvgFile(string exportPath)
        {
            _rootGraph.ComputeLayout();
            _rootGraph.ToSvgFile(exportPath);             
            _rootGraph.FreeLayout();
        }

        public void ExportToPngFile(string exportPath)
        {
            _rootGraph.ComputeLayout();
            _rootGraph.ToPngFile(exportPath);
            _rootGraph.FreeLayout();
        }
    }
}
