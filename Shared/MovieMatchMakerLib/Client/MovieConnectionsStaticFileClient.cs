using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;
using MovieMatchMakerLib.Model.Context;

namespace MovieMatchMakerLib.Client
{
    public class MovieConnectionsStaticFileClient : IMovieConnectionsClient
    {
        public const string MovieConnectionsFilename = "movieconnections.json";       

        private readonly IHttpClientFactory _httpClientFactory;

        private MovieConnection.List _movieConnections;

        private bool _applyDefaultFilters = true;

        private readonly ILogger<MovieConnectionsStaticFileClient> _logger;

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

        public Task<Stream> GetMovieConnectionsGraphForMovie(string title, int releaseYear)
        {
            throw new NotImplementedException();
        }

        private async Task<MovieConnection.List> LoadMovieConnectionsAsync()
        {
            if (_movieConnections is null)
            {
                _movieConnections = await DeserializeMovieConnections();
                //_movieConnections = await httpClient.GetFromJsonAsync<MovieConnection.List>(MovieConnectionsFilename, GlobalSerializerOptions.Options);
                if (_applyDefaultFilters)
                {
                    _movieConnections = _movieConnections.Filter(DefaultMovieConnectionListFilters.Filters);
                }
            }
            return _movieConnections;
        }

        private async Task<MovieConnection.List> DeserializeMovieConnections()
        {
            var stopWatch = new PrintStopwatch();
            var httpClient = _httpClientFactory.CreateClient("Static");

            stopWatch.Start();
            var json = await httpClient.GetStringAsync(MovieConnectionsFilename);
            stopWatch.Stop();
            _logger.LogInformation($"MovieConnections' json fectched in {stopWatch.ElapsedMilliseconds / 1000.0} s");

            stopWatch.Restart();            
            var options = GlobalSerializerOptions.Options;
            MovieConnection.List movieConnections = null;
            //options.TypeInfoResolver = MovieConnectionListJsonSerializerContext.Default;
            //movieConnections = JsonSerializer.Deserialize(json, typeof(MovieConnection.List), options) as MovieConnection.List;
            movieConnections = MovieConnection.List.FromJson(json);
            stopWatch.Stop();
            _logger.LogInformation($"MovieConnections deserialized in {stopWatch.ElapsedMilliseconds / 1000.0} s");

            return movieConnections;
        }
    }
}
