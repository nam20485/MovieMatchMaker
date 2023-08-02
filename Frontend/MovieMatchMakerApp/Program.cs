using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib;
using MovieMatchMakerLib.Utils;
using MovieMatchMakerLib.Data;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

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
                        case "--title": title = args[i+++1]; break;
                        case "--releaseYear": releaseYear = int.Parse(args[++i]); break;
                        case "--degree": degree = int.Parse(args[++i]); break;
                        case "--file": file = args[++i]; break;
                        case "--continue": continueExisting = bool.Parse(args[++i]); break;
                        case "--threaded": threaded = bool.Parse(args[++i]); break;
                    }
                }

                if (args[0] == "--build-connections")
                {
                    if (args.Length == 5)
                    {
                        if (!string.IsNullOrWhiteSpace(file))
                        {
                            ErrorLog.Reset();

                            if (await BuildMovieConnections(file, threaded))
                            {
                                return 0;
                            }
                        }
                    }
                }
                else if (args[0] == "--build-data")
                {
                    if (args.Length == 13 ||
                        args.Length == 9)       // no title and releaseYear
                    {
                        //--build - data--title "Dark City"--releaseYear 1998--degree 1--threaded true--file./ movie - data.json--continue false

                        ErrorLog.Reset();

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
                }
                //else if (args[0] == "--time")
                //{
                //    if (args.Length == 7)
                //    {
                //        if (!string.IsNullOrWhiteSpace(file))
                //        {
                //            if (TimeBuildingConnectionsAndApplyingFilters(file, threaded, continueExisting))
                //            {
                //                return 0;
                //            }
                //        }
                //    }
                //}                       
            }

            return 1;
        }

        private static async Task<bool> BuildMovieConnections(string file, bool threaded)
        {
            Console.Write("Loading movie data... ");

            var loadingAnimation = new ConsoleAnimation((fn) =>
            {
                return (fn % 4) switch
                {
                    0 => "/",
                    1 => "|",
                    2 => @"\",
                    3 => "-",                    
                };
            });
            
            loadingAnimation.Start();
            using var connectionBuilder = CreateMovieConnectionBuilder(file, threaded);
            loadingAnimation.Stop();
            loadingAnimation.Dispose();            

            Console.WriteLine(/**/);
            Console.WriteLine($"Movies:               {connectionBuilder.MoviesCount,5}\nMovie Credits:        {connectionBuilder.MovieCreditsCount,5}\nPerson Movie Credits: {connectionBuilder.PersonMovieCreditsCount,5}");            

            Console.WriteLine(/**/);
            Console.WriteLine("Building movie connections...");
            connectionBuilder.Start();
            await connectionBuilder.FindMovieConnections();            

            if (threaded)
            {
                Console.WriteLine();
                Console.WriteLine("(Press CTRL+m to quit)");

                using (var timerAnimation = new ConsoleAnimation(0, 9, (fn) =>
                {
                    return $"MovieConnections found: {connectionBuilder.MovieConnectionsFound,5:0.} ({connectionBuilder.MovieConnectionsFoundPerSecond,6:0.0}/s) ";
                }))
                {
                    timerAnimation.Start();
                    WaitForExitChar();                    
                }
            }

            Console.WriteLine("\n\nStopping...");
            connectionBuilder.Stop();

            const string movieConnectionsFilePath = "./movie-connections.json";
            Console.WriteLine($"\nSaving ({movieConnectionsFilePath})...");            
            connectionBuilder.SaveMovieConnections(movieConnectionsFilePath);

            return true;
        }

        private static void WaitForExitChar()
        {
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

                using (var timerAnimation = new ConsoleAnimation(0, 6, (fn) =>
                {
                    return $"Movies:        {movieDataBuilder.MoviesFetched,5:#.##} ({movieDataBuilder.MoviesFetchPerSecond,6:0.0}/s)    \nMovieCredits:  {movieDataBuilder.MovieCreditsFetched,5:0.##} ({movieDataBuilder.MovieCreditsFetchPerSecond,6:0.0}/s)    \nPersonCredits: {movieDataBuilder.PersonMovieCreditsFetched,5:0.##} ({movieDataBuilder.PersonMovieCreditsFetchPerSecond,6:0.0}/s) \n------------------------------- \nTotal:         {movieDataBuilder.TotalFetched,5:0.##} ({movieDataBuilder.TotalFetchPerSecond,6:0.0}/s)\n\nRunning for {movieDataBuilder.RunningTime:hh\\:mm\\:ss\\:ff} ";
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
                }
            }

            Console.WriteLine("\n\nStopping...");
            movieDataBuilder.Stop();

            if (movieDataBuilder.TaskCount > 0)
            {
                using (var remainingTasksAnimation = new ConsoleAnimation(0, 12, (fn) =>
                {
                    return $"Waiting for tasks to complete ({movieDataBuilder.TaskCount})... ";
                }))
                {
                    remainingTasksAnimation.Start();
                }

                Console.WriteLine();
            }

            //Console.WriteLine($"\nFinished (ran for {movieDataBuilder.RunTime:hh\\:mm\\:ss\\:ff}).");

            return true;
        }

        //private static bool TimeBuildingConnectionsAndApplyingFilters(string file, bool threaded, bool continueExisting)
        //{
        //    var stopWatch = new PrintStopwatch();

        //    var connectionBuilder = CreateMovieConnectionBuilder(file, threaded);

        //    bool movieConnectionsLoaded = false;
        //    try
        //    {
        //        if (continueExisting)
        //        {
        //            stopWatch.Start("Loading movie connections from file... ", false);

        //            connectionBuilder.LoadMovieConnections(MovieConnectionBuilder.FilePath);
        //            movieConnectionsLoaded = true;

        //            stopWatch.Stop("loaded");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Console.WriteLine("failed");
        //    }

        //    if (!movieConnectionsLoaded)
        //    {
        //        stopWatch.Start("Finding movie connections... ", false);

        //        connectionBuilder.Start();
        //        connectionBuilder.FindMovieConnections().Wait();
        //        connectionBuilder.Wait();
        //        connectionBuilder.Stop();

        //        stopWatch.Stop("complete");

        //        stopWatch.Start("Saving movie connections to file... ", false);

        //        connectionBuilder.SaveMovieConnections(MovieConnectionBuilder.FilePath);

        //        stopWatch.Stop("saved");
        //    }

        //    Console.WriteLine($"Found {connectionBuilder.MovieConnections.Count} movie connections");

        //    if (connectionBuilder.MovieConnections.Count > 0)
        //    {
        //        //await _connectionManager.FindMoviesConnectedToMovie("Dark City", 1998, 1);
        //        //await _dataCache.SaveAsync();

        //        stopWatch.Start("Applying filters... ", false);

        //        var sorted = new SortFilter().Apply(connectionBuilder.MovieConnections);
        //        var greaterThan2ConnectedRoles = new MinConnectedRolesCountFilter(2).Apply(connectionBuilder.MovieConnections);
        //        var greaterThan4ConnectedRoles = new MinConnectedRolesCountFilter(4).Apply(connectionBuilder.MovieConnections);
        //        var greaterThan8ConnectedRoles = new MinConnectedRolesCountFilter(8).Apply(connectionBuilder.MovieConnections);
        //        var greaterThan16ConnectedRoles = new MinConnectedRolesCountFilter(16).Apply(connectionBuilder.MovieConnections);
        //        var greaterThan32ConnectedRoles = new MinConnectedRolesCountFilter(32).Apply(connectionBuilder.MovieConnections);
        //        var greaterThan64ConnectedRoles = new MinConnectedRolesCountFilter(64).Apply(connectionBuilder.MovieConnections);
        //        var max0MatchingTitleWords = new MaxMatchingTitleWordsFilter(0).Apply(connectionBuilder.MovieConnections);
        //        var max1MatchingTitleWords = new MaxMatchingTitleWordsFilter(1).Apply(connectionBuilder.MovieConnections);
        //        var max2MatchingTitleWords = new MaxMatchingTitleWordsFilter(2).Apply(connectionBuilder.MovieConnections);

        //        var allFilters =
        //            new MaxMatchingTitleWordsFilter().Apply(
        //                new MinConnectedRolesCountFilter().Apply(
        //                    new SortFilter().Apply(connectionBuilder.MovieConnections)));

        //        stopWatch.Stop("finished");
        //        return true;
        //    }

        //    return false;
        //}

        private static IMovieConnectionBuilder CreateMovieConnectionBuilder(string filePath, bool threaded)
        {
            var dataCache = JsonFileCache.Load(filePath);
            if (threaded)
            {
                return new ThreadedMovieConnectionBuilder(dataCache);
            }
            else
            {
                return new MovieConnectionBuilder(dataCache);
            }
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