using MovieMatchMakerLib;
using MovieMatchMakerLib.Utils;
using MovieMatchMakerLib.Data;


namespace MovieMatchMakerApp
{
    internal class Program
    {
        private const string MovieConnectionsFilePath = "./movie-connections.json";

        public static ConsoleLogger ConsoleLogger { get; }
        public static FileLogger FileLogger { get; }

        static Program()
        {
            ConsoleLogger = new ();
            FileLogger = new ();
        }

        static async Task<int> Main(string[] args)
        {
            var exitCode = ExitCode.UnknownError;

            ConsoleLogger.Start();
            FileLogger.Start();
            ErrorLog.Reset();

            Console.WriteLine(Constants.Strings.HeaderWithVersion + Environment.NewLine);                               

            var mmmArgs = new MmmCliArguments(args);            
            if (mmmArgs.BuildConnections && mmmArgs.Count() >= 2)
            {
                //--build-connections --threaded true --file ./movie-data.json
                //--build-connections --threaded --file ./movie-data.json

                if (!string.IsNullOrWhiteSpace(mmmArgs.File))
                {
                    if (await BuildMovieConnections(mmmArgs.File, mmmArgs.Threaded))
                    {
                        exitCode = ExitCode.Success;
                    }
                }
            }
            else if (mmmArgs.BuildData && mmmArgs.Count() >= 4)
            {
                //--build-data --title "Dark City" --releaseYear 1998 --degree 1 --threaded true --file ./movie-data.json --continue-existing false
                //--build-data --title "Dark City" --releaseYear 1998 --degree 1 --threaded --file ./movie-data.json

                if (!string.IsNullOrWhiteSpace(mmmArgs.File))
                {
                    if (await BuildMovieDataAsync(mmmArgs.Title, mmmArgs.ReleaseYear, mmmArgs.Degree, mmmArgs.File, mmmArgs.Threaded, mmmArgs.ContinueExisting))
                    {
                        exitCode = ExitCode.Success;
                    }
                }
            }
            //else if (mmmArgs.Timing && args.Length >= 1)
            //{
            //    if (!string.IsNullOrWhiteSpace(mmmArgs.File))
            //    {
            //        if (TimeBuildingConnectionsAndApplyingFilters(mmmArgs.File, mmmArgs.Threaded, mmmArgs.ContinueExisting))
            //        {
            //            exitCode = ExitCode.Success;
            //        }
            //    }
            //}
            else
            {
                exitCode = ExitCode.InvalidArguments;
            }

            ConsoleLogger.Dispose();
            FileLogger.Dispose();

            return (int)exitCode;
        }

        /// <summary>
        /// Finds all connections between all movies found in the data from a movie-data.json file. 
        /// (Previously-built using a MovieDataBuilder)
        /// </summary>
        /// <param name="file">the movie-data.json file created by an <see cref="IMovieDataBuilder"/></param>
        /// <param name="threaded">use a multi-threaded or single-threaded <see cref="IMovieConnectionBuilder"/></param>
        /// <returns><see langword="true"/> on success, <see langword="false"/> otherwise</returns>
        private static async Task<bool> BuildMovieConnections(string file, bool threaded)
        {
            Console.Write("Loading movie data");

            using var loadingAnimation = new EllipsisAnimation()
            {
                LastFrame = "..."
            };     
            loadingAnimation.Start();
            using var connectionBuilder = CreateMovieConnectionBuilder(file, threaded);
            loadingAnimation.Stop();
            
            Console.WriteLine($"\n\nMovies:               {connectionBuilder.MoviesCount,6}\nMovie Credits:        {connectionBuilder.MovieCreditsCount,6}\nPerson Movie Credits: {connectionBuilder.PersonMovieCreditsCount,6} ");

            using (var consoleColors = new ConsoleColors(ConsoleColor.DarkGray))
            {
                Console.WriteLine("\n(Press CTRL+m to quit)");
            }

            Console.WriteLine("\nBuilding movie connections...");
            
            connectionBuilder.Start();
            await connectionBuilder.FindMovieConnections();            

            if (threaded)
            {                           
                using var timerAnimation = new ConsoleAnimation(0, 12, (fn) =>
                {
                    return $"MovieConnections found: {connectionBuilder.MovieConnectionsFound,5:0.} ({connectionBuilder.MovieConnectionsFoundPerSecond,6:0.0}/s) \n\nRunning for {connectionBuilder.RunningTime:hh\\:mm\\:ss\\:ff} ";
                });
                
                timerAnimation.Start();
                WaitForExitChar();
                timerAnimation.Stop();
            }

            connectionBuilder.Stop();
            Console.WriteLine($"\n\nSaving to {MovieConnectionsFilePath}...");            
            connectionBuilder.SaveMovieConnections(MovieConnectionsFilePath);

            return true;
        }

        /// <summary>
        /// Uses the <see href="https://www.tmdb.org">TMDB</see> API to build movie data starting from an initial "seed" movie.
        /// <para>Movie data includes title, release year, API ids, credits, and person's credits.</para>
        /// </summary>
        /// <param name="title">movie title</param>
        /// <param name="releaseYear">year movie was release</param>
        /// <param name="degree">recursive degree (specify 0 to get just one movie, > 0 to fetch movies connected to the intial movie amd so on...)</param>
        /// <param name="file">output path and file name to write data to, also can specify an existing file to start with</param>
        /// <param name="threaded"><see langword="true"/> for multi-threaded builder, <see langword="false"/> for single-threaded</param>
        /// <param name="continueExisting">build further starting with data already existing in file specified</param>
        /// <returns></returns>
        private static async Task<bool> BuildMovieDataAsync(string title, int releaseYear, int degree, string file, bool threaded, bool continueExisting)
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
                using (var consoleColors = new ConsoleColors(ConsoleColor.DarkGray))
                {
                    Console.WriteLine("(Press CTRL+m to quit)");
                }

                using (var timerAnimation = new ConsoleAnimation(0, 6, (fn) =>
                {
                    return $"Movies:        {movieDataBuilder.MoviesFetched,5:#.##} ({movieDataBuilder.MoviesFetchPerSecond,6:0.0}/s)    \nMovieCredits:  {movieDataBuilder.MovieCreditsFetched,5:0.##} ({movieDataBuilder.MovieCreditsFetchPerSecond,6:0.0}/s)    \nPersonCredits: {movieDataBuilder.PersonMovieCreditsFetched,5:0.##} ({movieDataBuilder.PersonMovieCreditsFetchPerSecond,6:0.0}/s) \n------------------------------- \nTotal:         {movieDataBuilder.TotalFetched,5:0.##} ({movieDataBuilder.TotalFetchPerSecond,6:0.0}/s)\n\nRunning for {movieDataBuilder.RunningTime:hh\\:mm\\:ss\\:ff} ";
                }))
                {
                    timerAnimation.Start();
                    WaitForExitChar();
                    timerAnimation.Stop();
                }
            }

            Console.WriteLine();
            //Console.WriteLine("\nStopping...");
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

        private static void WaitForExitChar()
        {
            // CTRL+m to exit
            WaitForExitChar(ConsoleKey.M, ConsoleModifiers.Control);
        }

        private static void WaitForExitChar(ConsoleKey key, ConsoleModifiers modifiers)
        {
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == key &&
                    keyInfo.Modifiers == modifiers)
                {
                    // key pressed, exit
                    break;
                }
            }
        }

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
    }
}