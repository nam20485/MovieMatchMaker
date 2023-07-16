namespace MovieMatchMakerLibTests
{
    public class MovieConnectionBuilderTests
    {      
        [Fact]
        public void Test_LoadMovieConnections()
        {
            var movieConnectionBuilder = Utils.CreateMovieConnectionBuilder(true);
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
