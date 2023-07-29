using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Graph;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib;

namespace MovieMatchMakerLibTests
{
    public class MovieConnectionsClusterTests
    {
        [Fact]
        public void Test_ExportGraphForMoviesConnections_Png()
        {
            const string ExportPath = "connections_cluster.png";

            File.Exists(MovieConnectionBuilder.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilder.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);
          
            var filtered = loaded.Filter(new MinConnectedRolesCountFilter(14));
            filtered.Should().NotBeNull();
            filtered.Should().NotBeEmpty();
            //filtered.Should().HaveCount(236);

            var cluster = new MovieConnectionsCluster(filtered);
            cluster.Should().NotBeNull();
            cluster.BuildCluster();
            cluster.ExportToPngFile(ExportPath);
            File.Exists(ExportPath).Should().BeTrue();         
        }
    }
}
