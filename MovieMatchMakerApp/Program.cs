using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib;
using MovieMatchMakerLib.Utils;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerApp
{
    internal class Program
    {
        private static readonly MovieNetworkDataBuilder _movieNetworkDataBuilder;        
        private static readonly MovieConnectionBuilder _connectionBuilder;

        static Program()
        {
            var apiDataSource = new ApiDataSource();
            var dataCache = JsonFileCache.Load(MovieDataBuilderBase.FilePath);            
            var cachedDataSource = new CachedDataSource(dataCache, apiDataSource);

            _movieNetworkDataBuilder = new MovieNetworkDataBuilder(cachedDataSource);
            _connectionBuilder = new MovieConnectionBuilder(dataCache);                        
        }     

        static int Main(string[] args)
        {
            Console.WriteLine(Constants.Strings.Header);
            Console.WriteLine(/* blank line for separation */);

            var stopWatch = new PrintStopwatch();

            //await _connectionManager.FindMoviesConnectedToMovie("Dark City", 1998, 1);
            //await _dataCache.SaveAsync();                                   

            bool movieConnectionsLoaded = false;
            try
            {
                stopWatch.Start("Loading movie connections from file... ", false);

                _connectionBuilder.LoadMovieConnections(MovieConnectionBuilderBase.FilePath);
                movieConnectionsLoaded = true;                

                stopWatch.Stop("loaded");
            }
            catch (Exception)
            {
                Console.WriteLine("failed");
            }

            if (!movieConnectionsLoaded)
            {
                stopWatch.Start("Finding movie connections... ", false);

                _connectionBuilder.FindMovieConnections().Wait();

                stopWatch.Stop("complete");

                stopWatch.Start("Saving movie connections to file... ", false);

                _connectionBuilder.SaveMovieConnections(MovieConnectionBuilderBase.FilePath);

                stopWatch.Stop("saved");
            }

            Console.WriteLine($"Found {_connectionBuilder.MovieConnections.Count} movie connections");

            if (_connectionBuilder.MovieConnections.Count > 0)
            {
                stopWatch.Start("Applying filters... ", false);

                var sorted = new SortFilter().Apply(_connectionBuilder.MovieConnections);
                var greaterThan2ConnectedRoles = new MinConnectedRolesCountFilter(2).Apply(_connectionBuilder.MovieConnections);
                var greaterThan4ConnectedRoles = new MinConnectedRolesCountFilter(4).Apply(_connectionBuilder.MovieConnections);
                var greaterThan8ConnectedRoles = new MinConnectedRolesCountFilter(8).Apply(_connectionBuilder.MovieConnections);
                var greaterThan16ConnectedRoles = new MinConnectedRolesCountFilter(16).Apply(_connectionBuilder.MovieConnections);
                var greaterThan32ConnectedRoles = new MinConnectedRolesCountFilter(32).Apply(_connectionBuilder.MovieConnections);
                var greaterThan64ConnectedRoles = new MinConnectedRolesCountFilter(64).Apply(_connectionBuilder.MovieConnections);
                var max0MatchingTitleWords = new MaxMatchingTitleWordsFilter(0).Apply(_connectionBuilder.MovieConnections);
                var max1MatchingTitleWords = new MaxMatchingTitleWordsFilter(1).Apply(_connectionBuilder.MovieConnections);
                var max2MatchingTitleWords = new MaxMatchingTitleWordsFilter(2).Apply(_connectionBuilder.MovieConnections);

                var allFilters =
                    new MaxMatchingTitleWordsFilter().Apply(
                        new MinConnectedRolesCountFilter().Apply(
                            new SortFilter().Apply(_connectionBuilder.MovieConnections)));

                stopWatch.Stop("finished");

                return 0;
            }

            return 1;
        }
    }
}