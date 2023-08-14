using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib.Client
{
    public class MovieConnectionsStaticFileClient : IMovieConnectionsClient
    {
        public const string MovieConnectionsFilename = "movie-connections.json";       

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MovieConnectionsStaticFileClient> _logger;

        private MovieConnection.List _movieConnections;

        private readonly bool _applyDefaultFilters = true;

        private const string HttpClientName = "Static";

        public HttpClient HttpClient => _httpClientFactory.CreateClient(HttpClientName);

        public MovieConnectionsStaticFileClient(IHttpClientFactory httpClientFactory, ILogger<MovieConnectionsStaticFileClient> logger)
        {
            _logger = logger;
            _movieConnections = null;
            _httpClientFactory = httpClientFactory;            
        }

        public async Task<MovieConnection.List> FilterAllMovieConnections(IEnumerable<IMovieConnectionListFilter> filters)
        {
            return (await GetAllMovieConnections()).Filter(filters);
        }       

        public async Task<MovieConnection.List> FilterMovieConnectionsForMovie(string title, int releaseYear, IEnumerable<IMovieConnectionListFilter> filters)
        {
            return (await GetMovieConnectionsForMovie(title, releaseYear)).Filter(filters);
        }

        public async Task<MovieConnection.List> GetAllMovieConnections()
        {
            return await LoadMovieConnectionsAsync();
        }

        public async Task<MovieConnection> GetMovieConnection(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle, int targetMovieReleaseYear)
        {
            return (await GetAllMovieConnections()).FindConnection(sourceMovieTitle, sourceMovieReleaseYear, targetMovieTitle, targetMovieReleaseYear);
        }

        public async Task<MovieConnection> GetMovieConnection(int movieConnectionId)
        {
            return (await GetAllMovieConnections()).FindConnection(movieConnectionId);
        }

        public async Task<MovieConnection.List> GetMovieConnectionsForMovie(string title, int releaseYear)
        {
            return (await GetAllMovieConnections()).FindForMovie(title, releaseYear);
        }

        public Task<string> GetMovieConnectionsGraphForMovie(string title, int releaseYear)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAllMovieConnectionsGraph()
        {
            throw new NotImplementedException();
        }

        private async Task<MovieConnection.List> LoadMovieConnectionsAsync()
        {
            if (_movieConnections is null)
            {
                using var httpClient = HttpClient;         
                _movieConnections = await httpClient.GetFromJsonAsync<MovieConnection.List>(MovieConnectionsFilename, GlobalSerializerOptions.Options);
                //_movieConnections = await DeserializeMovieConnections();
                if (_applyDefaultFilters)
                {
                    _movieConnections = _movieConnections.Filter(DefaultMovieConnectionListFilters.Filters);
                }
            }
            return _movieConnections;
        }

        protected async Task<MovieConnection.List> DeserializeMovieConnections()
        {
            var stopWatch = new PrintStopwatch();
            using var httpClient = HttpClient;

            stopWatch.Start();
            var json = await httpClient.GetStringAsync(MovieConnectionsFilename);
            stopWatch.Stop();
            _logger.LogInformation("MovieConnections' json fectched in {Duration} s", stopWatch.ElapsedMilliseconds / 1000.0);

            stopWatch.Restart();
            MovieConnection.List movieConnections;
            //movieConnections = JsonSerializer.Deserialize(json, typeof(MovieConnection.List), new MovieConnectionListJsonSerializerContext(GlobalSerializerOptions.Options)) as MovieConnection.List;
            movieConnections = MovieConnection.List.FromJson(json);
            stopWatch.Stop();
            _logger.LogInformation("MovieConnections deserialized in {Duration} s", stopWatch.ElapsedMilliseconds / 1000.0);

            return movieConnections;
        }    
    }
}
