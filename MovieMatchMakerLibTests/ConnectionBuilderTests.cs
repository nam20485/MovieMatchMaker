using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using MovieMatchMakerLib;
using MovieMatchMakerLib.DataCache;
using MovieMatchMakerLib.DataSource;

namespace MovieMatchMakerLibTests
{
    public class ConnectionBuilderTests
    {      
        [Fact]
        public void Test_LoadMovieConnections()
        {
            var movieConnectionBuilder = Utils.CreateMovieConnectionBuilder();
            movieConnectionBuilder.LoadMovieConnections(MovieConnectionBuilderBase.FilePath);
            movieConnectionBuilder.MovieConnections.Should().HaveCount(17413);                       
        }

    }

    //bool movieConnectionsLoaded = false;
    //            try
    //            {
    //                stopWatch.Start("Loading movie connections from file... ", false);

    //                _connectionBuilder.LoadMovieConnections(MovieConnectionsFilePath);
    //                movieConnectionsLoaded = true;                

    //                stopWatch.Stop("loaded");
    //            }
    //            catch (Exception)
    //            {
    //    Console.WriteLine("failed");
    //}

    //if (!movieConnectionsLoaded)
    //{
    //    stopWatch.Start("Finding movie connections... ", false);

    //    _connectionBuilder.FindMovieConnections().Wait();
    //    stopWatch.Stop("complete");

    //    stopWatch.Start("Saving movie connections to file... ", false);

    //    _connectionBuilder.SaveMovieConnections(MovieConnectionsFilePath);

    //    stopWatch.Stop("saved");
}
