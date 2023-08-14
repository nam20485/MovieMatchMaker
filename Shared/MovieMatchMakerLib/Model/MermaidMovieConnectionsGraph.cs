using System.Collections.Generic;
using System.Text;

using MovieMatchMakerApi.Controllers;

namespace MovieMatchMakerLib.Model
{
    public class MermaidMovieConnectionsGraph : MovieConnectionsGraph<string>
    {
        public bool UseElkRenderer { get; set; }
        public bool UseDirectedEdges { get; set; }

        private const string _directedEdgeSymbol = "-->";
        private const string _undirectedEdgeSymbol = "---";

        public MermaidMovieConnectionsGraph(IEnumerable<MovieConnection> movieConnections)
            : base(movieConnections)
        {
            UseElkRenderer = true;
            UseDirectedEdges = true;
        }

        protected override string BuildGraph()
        {
            // graph LR
            // A[Hard]-- >| Text | B[Round]
            var sb = new StringBuilder();
            if (UseElkRenderer)
            {
                sb.AppendLine("""%%{init: {"flowchart": {"defaultRenderer": "elk"}} }%%""");
            }
            sb.AppendLine("graph LR");
            _movieConnections.ForEach(c => sb.AppendLine(MakeConnection(c)));          
            return sb.ToString();
        }

        private string MakeConnection(MovieConnection connection)
        {
            // e.g. A[Hard]-->|Text|B[Round]
            var sourceMovie = connection.SourceMovie;
            var targetMovie = connection.TargetMovie;
            var edgeLabel = connection.ConnectedRoles.Count.ToString();
            return $"{MakeVertex(sourceMovie)} {MakeEdge(edgeLabel)} {MakeVertex(targetMovie)}";
        }

        private string MakeEdge(string label) => $"{(UseDirectedEdges? _directedEdgeSymbol : _undirectedEdgeSymbol)}|{label}|";
        private static string MakeVertex(Movie movie) => $"{movie.GetHashCode()}({movie.Title})";
    }
}
