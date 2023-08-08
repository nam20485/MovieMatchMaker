using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib;

namespace MovieMatchMakerLibTests
{
    public class MovieConnectionsClusterTests
    {
        // [Fact]
        // public void Test_ExportGraphForMoviesConnections_Png()
        // {
        //     const string ExportPath = "connections_cluster.png";

        //     File.Exists(Constants.Strings.MovieConnectionsFilePath).Should().BeTrue();
        //     var loaded = MovieConnection.List.LoadFromFile(Constants.Strings.MovieConnectionsFilePath);
        //     loaded.Should().NotBeNull();
        //     loaded.Should().NotBeEmpty();
        //     loaded.Should().HaveCount(11754);
          
        //     var filtered = loaded.Filter(new MinConnectedRolesCountFilter(14));
        //     filtered.Should().NotBeNull();
        //     filtered.Should().NotBeEmpty();
        //     //filtered.Should().HaveCount(236);

        //     var cluster = new MovieConnectionsCluster(filtered);
        //     cluster.Should().NotBeNull();
        //     cluster.BuildCluster();
        //     cluster.ExportToPngFile(ExportPath);
        //     File.Exists(ExportPath).Should().BeTrue();         
        // }
    }
}
