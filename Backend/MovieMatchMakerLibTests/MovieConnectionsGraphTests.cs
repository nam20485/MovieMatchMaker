using MovieMatchMakerLib.Model;
using MovieMatchMakerLib;
using MovieMatchMakerLib.Graph;
using MovieMatchMakerLib.Filters;

namespace MovieMatchMakerLibTests
{
    public class MovieConnectionsGraphTests
    {
    //    [Fact]
    //    public void Test_ExportGraphForMoviesConnections_Svg_DarkCity_1998()
    //    {
    //        File.Exists(Constants.Strings.MovieConnectionsFilePath).Should().BeTrue();
    //        var loaded = MovieConnection.List.LoadFromFile(Constants.Strings.MovieConnectionsFilePath);
    //        loaded.Should().NotBeNull();
    //        loaded.Should().NotBeEmpty();
    //        loaded.Should().HaveCount(11754);

    //        var movieConnectionsForDarkCity = loaded.FindForMovie(Constants.Strings.DarkCityTitle, Constants.Ints.DarkCityReleaseYear);
    //        movieConnectionsForDarkCity.Should().NotBeNull();
    //        movieConnectionsForDarkCity.Should().NotBeEmpty();
    //        movieConnectionsForDarkCity.Should().HaveCount(532);
    //        var filtered = movieConnectionsForDarkCity.Filter(new MinConnectedRolesCountFilter(6));

    //        var graph = new MovieConnectionsGraph(filtered);
    //        graph.Should().NotBeNull();
    //        const string ExportPath = "darkcity_1998_connections.svg";
    //        graph.ExportToSvgFile(ExportPath);
    //        File.Exists(ExportPath).Should().BeTrue();
    //    }

    //    [Fact]
    //    public void Test_ExportGraphForMoviesConnections_Png_DarkCity_1998()
    //    {
    //        const string ExportPath = "darkcity_1998_connections.png";

    //        File.Exists(Constants.Strings.MovieConnectionsFilePath).Should().BeTrue();
    //        var loaded = MovieConnection.List.LoadFromFile(Constants.Strings.MovieConnectionsFilePath);
    //        loaded.Should().NotBeNull();
    //        loaded.Should().NotBeEmpty();
    //        loaded.Should().HaveCount(11754);

    //        var movieConnectionsForDarkCity = loaded.FindForMovie(Constants.Strings.DarkCityTitle, Constants.Ints.DarkCityReleaseYear);
    //        movieConnectionsForDarkCity.Should().NotBeNull();
    //        movieConnectionsForDarkCity.Should().NotBeEmpty();
    //        movieConnectionsForDarkCity.Should().HaveCount(532);
    //        var filtered = movieConnectionsForDarkCity.Filter(new MinConnectedRolesCountFilter(6));

    //        var graph = new MovieConnectionsGraph(filtered);
    //        graph.Should().NotBeNull();            
    //        graph.ExportToPngFile(ExportPath);
    //        File.Exists(ExportPath).Should().BeTrue();
    //    }
    }
}
