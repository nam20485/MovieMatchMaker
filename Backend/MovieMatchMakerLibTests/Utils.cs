using Microsoft.Extensions.Logging;

using MovieMatchMakerApi.Controllers;
using MovieMatchMakerApi.Services;

using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerLibTests
{
    public class Utils
    {
        public static JsonFileCache LoadJsonFileCache()
        {
            var dataCache = JsonFileCache.Load(GetTestMovieDataFilePath());
            dataCache.Should().NotBeNull();
            return dataCache;
        }

        public static MovieConnectionBuilder CreateMovieConnectionBuilder(bool loaded)
        {
            var dataCache = LoadJsonFileCache();
            var connectionBuilder = new MovieConnectionBuilder(dataCache);
            if (loaded)
            {
                connectionBuilder.LoadMovieConnections(GetTestMovieConnectionsFilePath());
            }
            return connectionBuilder;
        }        

        public static CachedDataSource CreateCachedDataSource()
        {
            var dataCache = LoadJsonFileCache();           
            var apiDataSource = new ApiDataSource();
            var dataSource = new CachedDataSource(dataCache, apiDataSource);
            return dataSource;
        }  
        
        public static MovieDataBuilder CreateMovieDataBuilder()
        {            
            var cachedDataSource = CreateCachedDataSource();
            var movieNetworkDataBuilder = new MovieDataBuilder(cachedDataSource);
            return movieNetworkDataBuilder;
        }

        public static MovieConnectionsController CreateMovieConnectionsController(bool applyDefaultFilters)
        {
            return new MovieConnectionsController(CreateLogger<MovieConnectionsController>(),
                                                  CreateMovieConnectionsService(),
                                                  applyDefaultFilters);
        }

        public static MovieConnectionsService CreateMovieConnectionsService()
        {
            return new MovieConnectionsService(CreateLogger<MovieConnectionsService>());
        }

        public static ILogger<T> CreateLogger<T>()
        {
            return LoggerFactory.Create(configure =>
            {
                //
            }).CreateLogger<T>();
        }

        public static string? GetTestDataDir()
        {
            const string testDataEnvVarName = Constants.Strings.TestDataDirEnvVarName;
            var testDataDir = Environment.GetEnvironmentVariable(testDataEnvVarName);

            testDataDir.Should().NotBeNull($"no {testDataEnvVarName} environment variable was found");
            testDataDir.Should().NotBeEmpty();            
            Directory.Exists(testDataDir).Should().BeTrue();
            
            return testDataDir;
        }

        public static string MakeTestDataFilePath(string filename)
        {
            // guaranteed !null b/c of asserts inside GetTestDataDir()
            var path = Path.Combine(GetTestDataDir()!, filename);

            path.Should().NotBeNull();
            path.Should().NotBeEmpty();
            File.Exists(path).Should().BeTrue();

            return path;
        }

        public static string GetTestMovieDataFilePath()
        {
            return MakeTestDataFilePath(Constants.Strings.MovieDataFilename);
        }

        public static string GetTestMovieConnectionsFilePath()
        {
            return MakeTestDataFilePath(Constants.Strings.MovieConnectionsFilename);
        }
    }
}
