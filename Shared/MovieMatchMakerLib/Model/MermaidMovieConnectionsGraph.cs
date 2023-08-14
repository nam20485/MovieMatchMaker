using System.Text;

using MovieMatchMakerApi.Controllers;

namespace MovieMatchMakerLib.Model
{
    public class MermaidMovieConnectionsGraph : MovieConnectionsGraph<string>
    {
        public bool UseElkRenderer { get; set; }

        public MermaidMovieConnectionsGraph(MovieConnection.List movieConnections)
            : base(movieConnections)
        {
            UseElkRenderer = true;
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

        private static string MakeConnection(MovieConnection connection)
        {
            // A[Hard]-->|Text|B[Round]
            var sourceMovie = connection.SourceMovie;
            var targetMovie = connection.TargetMovie;
            var edgeLabel = connection.ConnectedRoles.Count.ToString();
            return $"{MakeVertex(sourceMovie)} {MakeEdge(edgeLabel)} {MakeVertex(targetMovie)}";
        }

        private static string MakeEdge(string label) => $"-->|{label}|";
        private static string MakeVertex(Movie movie) => $"{movie.GetHashCode()}({movie.Title})";
    }
}
