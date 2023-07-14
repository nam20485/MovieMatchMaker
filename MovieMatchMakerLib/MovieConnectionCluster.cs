using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatchMakerLib
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
