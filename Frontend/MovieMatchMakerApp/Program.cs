using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib;
using MovieMatchMakerLib.Utils;
using MovieMatchMakerLib.Data;


namespace MovieMatchMakerApp
{
    internal class Program
    {        
        static Program()
        {                    
        }

        static async Task<int> Main(string[] args)
        {
            Console.WriteLine(Constants.Strings.HeaderWithVersion);
            Console.WriteLine(/* blank line for separation */);

            if (args.Length > 0)
            {
                if (args[0] == "--build-connections")
                {
                    if (await BuildMovieConnections())
                    {
                        return 0;
                    }
                }
                else if (args[0] == "--build-data")
                {
                    if (args.Length == 13 ||
                        args.Length == 9)       // no title and releaseYear
                    {
                        ErrorLog.Reset();

                        var title = "";
                        var releaseYear = -1;
                        var degree = -1;
                        var threaded = false;
                        var file = "";
                        var continueExisting = true;
                        for (int i = 1; i < args.Length - 1; i++)
                        {
                            switch (args[i])
                            {
                                case "--title": title = args[i++ + 1]; break;
                                case "--releaseYear": releaseYear = int.Parse(args[++i]); break;
                                case "--degree": degree = int.Parse(args[++i]); break;                                
                                case "--file": file = args[++i]; break;
                                case "--continue": continueExisting = bool.Parse(args[++i]); break;
                                case "--threaded": threaded = bool.Parse(args[++i]); break;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(title) &&
                            releaseYear > -1 &&
                            degree > -1 &&
                            !string.IsNullOrWhiteSpace(file))
                        {
                            if (await BuildMovieDataAsync(title, releaseYear, degree, file, threaded, continueExisting))
                            {
                                return 0;
                            }
                        }
                    }
                    else if (args[0] == "--time")
                    {
                        if (TimeBuildingConnectionsAndApplyingFilters())
                        {
                            return 0;
                        }
                    }
                }               
            }

            return 1;
        }

        private static async Task<bool> BuildMovieConnections()
        {
            Console.WriteLine("Building movie connections...");

            await CreateMovieConnectionBuilder(MovieDataBuilder.FilePath).FindMovieConnections();
            return true;
        }

        static async Task<bool> BuildMovieDataAsync(string title, int releaseYear, int degree, string file, bool threaded, bool continueExisting)
        {
            Console.WriteLine("Building movie data...");

            using var movieDataBuilder = CreateMovieDataBuilder(file, threaded, continueExisting);
            if (!continueExisting)
            {
                await movieDataBuilder.BuildFreshFromInitial(title, releaseYear, degree);
            }
            else
            {
                movieDataBuilder.ContinueFromExisting(degree);
            }

            if (threaded)
            {
                Console.WriteLine();
                Console.WriteLine("(Press CTRL+m to quit)");

                using (var timerAnimation = new ConsoleAnimation(0, 6, () =>
                {
                    return $"Movies:        {movieDataBuilder.MoviesFetched,5:#.##} ({movieDataBuilder.MoviesFetchPerSecond,5:0.00}/s)    \nMovieCredits:  {movieDataBuilder.MovieCreditsFetched,5:0.##} ({movieDataBuilder.MovieCreditsFetchPerSecond,5:0.00}/s)    \nPersonCredits: {movieDataBuilder.PersonMovieCreditsFetched,5:0.##} ({movieDataBuilder.PersonMovieCreditsFetchPerSecond,5:0.00}/s) ";
                }))
                {
                    timerAnimation.Start();

                    while (true)
                    {
                        var keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.M &&
                            keyInfo.Modifiers == ConsoleModifiers.Control)
                        {
                            // CTRL+m to exit
                            break;
                        }
                    }

                    timerAnimation.Stop();
                    movieDataBuilder.Stop();

                    Console.WriteLine("\n\nStopping...");

                    if (movieDataBuilder.TaskCount > 0)
                    {
                        using (var remainingTasksAnimation = new ConsoleAnimation(0, 12, () =>
                        {
                            return $"Waiting for tasks to complete ({movieDataBuilder.TaskCount})... ";
                        }))
                        {
                            remainingTasksAnimation.Start();
                        }

                        Console.WriteLine();
                    }

                    Console.WriteLine($"\nFinished (ran for {movieDataBuilder.RunTime:hh\\:mm\\:ss\\:ff}).");
                }
            }

            return true;
        }

        private static bool TimeBuildingConnectionsAndApplyingFilters()
        {
            var stopWatch = new PrintStopwatch();

            //await _connectionManager.FindMoviesConnectedToMovie("Dark City", 1998, 1);
            //await _dataCache.SaveAsync();

            var connectionBuilder = CreateMovieConnectionBuilder(MovieDataBuilder.FilePath);

            bool movieConnectionsLoaded = false;
            try
            {
                stopWatch.Start("Loading movie connections from file... ", false);

                connectionBuilder.LoadMovieConnections(MovieConnectionBuilder.FilePath);
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

                connectionBuilder.FindMovieConnections().Wait();

                stopWatch.Stop("complete");

                stopWatch.Start("Saving movie connections to file... ", false);

                connectionBuilder.SaveMovieConnections(MovieConnectionBuilder.FilePath);

                stopWatch.Stop("saved");
            }

            Console.WriteLine($"Found {connectionBuilder.MovieConnections.Count} movie connections");

            if (connectionBuilder.MovieConnections.Count > 0)
            {
                stopWatch.Start("Applying filters... ", false);

                var sorted = new SortFilter().Apply(connectionBuilder.MovieConnections);
                var greaterThan2ConnectedRoles = new MinConnectedRolesCountFilter(2).Apply(connectionBuilder.MovieConnections);
                var greaterThan4ConnectedRoles = new MinConnectedRolesCountFilter(4).Apply(connectionBuilder.MovieConnections);
                var greaterThan8ConnectedRoles = new MinConnectedRolesCountFilter(8).Apply(connectionBuilder.MovieConnections);
                var greaterThan16ConnectedRoles = new MinConnectedRolesCountFilter(16).Apply(connectionBuilder.MovieConnections);
                var greaterThan32ConnectedRoles = new MinConnectedRolesCountFilter(32).Apply(connectionBuilder.MovieConnections);
                var greaterThan64ConnectedRoles = new MinConnectedRolesCountFilter(64).Apply(connectionBuilder.MovieConnections);
                var max0MatchingTitleWords = new MaxMatchingTitleWordsFilter(0).Apply(connectionBuilder.MovieConnections);
                var max1MatchingTitleWords = new MaxMatchingTitleWordsFilter(1).Apply(connectionBuilder.MovieConnections);
                var max2MatchingTitleWords = new MaxMatchingTitleWordsFilter(2).Apply(connectionBuilder.MovieConnections);

                var allFilters =
                    new MaxMatchingTitleWordsFilter().Apply(
                        new MinConnectedRolesCountFilter().Apply(
                            new SortFilter().Apply(connectionBuilder.MovieConnections)));

                stopWatch.Stop("finished");
                return true;
            }

            return false;
        }

        private static IMovieConnectionBuilder CreateMovieConnectionBuilder(string filePath)
        {
            return new MovieConnectionBuilder(CreateJsonFileCache(filePath));
        }

        private static JsonFileCache CreateJsonFileCache(string filePath)
        {
            return JsonFileCache.Load(filePath);
        }

        private static IMovieDataBuilder CreateMovieDataBuilder(string cacheFilePath, bool threaded, bool load)
        {
            var dataSource = CachedDataSource.CreateWithJsonFileCacheAndApiDataSource(cacheFilePath, load);
            if (threaded)
            {
                return new ThreadedMovieDataBuilder(dataSource);
            }
            else
            {
                return new MovieDataBuilder(dataSource);
            }
        }
    }
}