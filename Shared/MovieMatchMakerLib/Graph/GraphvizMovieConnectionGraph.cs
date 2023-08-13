using MovieMatchMakerLib.Model;

using Rubjerg.Graphviz;

namespace MovieMatchMakerLib.Graph
{
    public class GraphvizMovieConnectionGraph
    {
        private readonly MovieConnection _movieConnection;
        private RootGraph _rootGraph;

        public GraphvizMovieConnectionGraph(MovieConnection movieConnection)
        {
            _movieConnection = movieConnection;
            _rootGraph = BuildGraph();
        }

        private RootGraph BuildGraph()
        {
            var rootGraph = RootGraph.CreateNew("Movie Connection", GraphType.Undirected);
            var sourceMovieNode = rootGraph.GetOrAddNode(_movieConnection.SourceMovie.DisplayId);
            var targetMovieNode = rootGraph.GetOrAddNode(_movieConnection.TargetMovie.DisplayId);

            foreach (var connectedRole in _movieConnection.ConnectedRoles)
            {
                //if (!connection.TargetMovie.Title.Contains("Die Helene Fischer"))
                {
                    var edge = rootGraph.GetOrAddEdge(sourceMovieNode, targetMovieNode, connectedRole.Name.FullName);
                    edge.SafeSetAttribute("label", connectedRole.Name.FullName, "");
                }
            }

            return rootGraph;
        }

        public void ExportToSvgFile(string exportPath)
        {
            _rootGraph.ComputeLayout();
            _rootGraph.ToSvgFile(exportPath);
            _rootGraph.FreeLayout();
        }
    }
}
