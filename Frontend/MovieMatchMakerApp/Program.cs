﻿using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib;
using MovieMatchMakerLib.Utils;
using MovieMatchMakerLib.Data;


namespace MovieMatchMakerApp
{
    internal class Program
    {
        private static IMovieConnectionBuilder CreateMovieConnectionBuilder(string filePath)
        {
            return new MovieConnectionBuilder(CreateJsonFileCache(filePath));
        }      

        private static JsonFileCache CreateJsonFileCache(string filePath)
        {
            return JsonFileCache.Load(MovieDataBuilder.FilePath);
        }

        private static IMovieDataBuilder CreateMovieDataBuilder(string cacheFilePath, bool threaded)
        {
            var dataSource = CachedDataSource.CreateWithJsonFileCacheAndApiDataSource(cacheFilePath, true);
            if (threaded)
            {
                return new ThreadedMovieDataBuilder(dataSource);
            }
            else
            {
                return new MovieDataBuilder(dataSource);
            }
        }

        static Program()
        {                    
        }     

        static async Task<int> Main(string[] args)
        {
            Console.WriteLine(Constants.Strings.Header);
            Console.WriteLine(/* blank line for separation */);

            if (args.Length > 0)
            {
                if (args[0] == "--build-connections")
                {           
                    Console.WriteLine("Building movie connections...");

                    await CreateMovieConnectionBuilder(MovieDataBuilder.FilePath).FindMovieConnections();
                    return 0;
                }
                else if (args[0] == "--build-data")
                {
                    if (args.Length == 9)
                    {
                        Console.WriteLine("Building movie data...");

                        var title = "";
                        var releaseYear = -1;
                        var degree = -1;
                        var threaded = false;
                        var file = "";
                        for (int i = 1; i < args.Length - 1; i++)
                        {
                            switch (args[i])
                            {
                                case "--title": title = args[i++ + 1]; break;
                                case "--releaseYear": releaseYear = int.Parse(args[i++ + 1]); break;
                                case "--degree": degree = int.Parse(args[i++ + 1]); break;
                                case "--threaded": threaded = bool.Parse(args[i++ + 1]); break;
                                case "--file": file = args[i++ + 1]; break;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(title) &&
                            releaseYear > -1 &&
                            degree > -1 &&
                            !string.IsNullOrWhiteSpace(file))
                        {
                            var movieDataBuilder = CreateMovieDataBuilder(file, threaded);
                            movieDataBuilder.BuildFromInitial(title, releaseYear, degree);
                            return 0;
                        }
                    }
                }
                else if (args[0] == "--time")
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
                        return 0;
                    }
                }
            }            

            return 1;
        }
    }
}