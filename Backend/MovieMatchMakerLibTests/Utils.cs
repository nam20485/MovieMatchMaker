using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

using Moq;

using MovieMatchMakerApi.Controllers;
using MovieMatchMakerApi.Services;

using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLibTests
{
    public class Utils
    {     
        private static MovieConnection.List LoadMovieConnections()
        {
            return LoadMovieConnections(Utils.GetTestMovieConnectionsFilePath());
        }

        private static MovieConnection.List LoadMovieConnections(string filePath)
        {
            return MovieConnection.List.LoadFromFile(filePath);
        }

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
            var mockMovieConnectionsService = new Mock<IMovieConnectionsService>();
            mockMovieConnectionsService
                .Setup(service => service.MovieConnections)
                .Returns(LoadMovieConnections());            

            var logger = CreateLogger<MovieConnectionsController>();

            return new MovieConnectionsController(logger,
                                                  mockMovieConnectionsService.Object,                                                  
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
