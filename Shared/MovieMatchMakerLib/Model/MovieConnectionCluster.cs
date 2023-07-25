namespace MovieMatchMakerLib.Model
{
    public class MovieConnectionCluster
    {
        public class Node
        {
            public Movie Parent { get; set; }
            public MovieConnection.List Connections { get; set; }
        }

    }
}
