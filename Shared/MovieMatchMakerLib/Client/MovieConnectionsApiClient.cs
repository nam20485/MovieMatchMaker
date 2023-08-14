using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;


namespace MovieMatchMakerLib.Client
{
    public class MovieConnectionsApiClient : IMovieConnectionsClient
    {
        private const string AllMovieConnectionsEndpoint = "MovieConnections/movieconnections";
        private const string MovieConnectionsForMovieEndpointFormat = "MovieConnections/movieconnections/{0}/{1}";
        private const string FilterAllMovieConnectionsEndpoint = "MovieConnections/movieconnections/filter";
        private const string FilterMovieConnectionsForMovieEndpointFormat = "MovieConnections/movieconnections/filter/{0}/{1}";
        private const string GetMovieConnectionByMoviesEndpointFormat = "MovieConnections/movieconnection/{0}/{1}/{2}/{3}";
        private const string GetMovieConnectionByIdEndpointFormat = "MovieConnections/movieconnection/{0}";
        private const string MovieConnectionsGraphForMovieEndpointFormat = "MovieConnections/movieconnections/graph/{0}/{1}";
        private const string AllMovieConnectionsGraphEndpointFormat = "MovieConnections/movieconnections/graph";  
        
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MovieConnectionsApiClient> _logger;

        private const string HttpClientName = "Api";
        public HttpClient HttpClient => _httpClientFactory.CreateClient(HttpClientName);

        public MovieConnectionsApiClient(IHttpClientFactory httpClientFactory, ILogger<MovieConnectionsApiClient> logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public virtual async Task<MovieConnection.List> GetAllMovieConnections()
        {
            using var httpClient = HttpClient;
            return await httpClient.GetFromJsonAsync<MovieConnection.List>(AllMovieConnectionsEndpoint, GlobalSerializerOptions.Options);
        }

        public virtual async Task<MovieConnection.List> GetMovieConnectionsForMovie(string title, int releaseYear)
        {            
            using var httpClient = HttpClient;
            var uri = string.Format(MovieConnectionsForMovieEndpointFormat, title, releaseYear);
            return await httpClient.GetFromJsonAsync<MovieConnection.List>(uri, GlobalSerializerOptions.Options);
        }

        public virtual async Task<MovieConnection.List> FilterAllMovieConnections(IEnumerable<IMovieConnectionListFilter> filters)
        {
            MovieConnection.List movieConnections = null;

            using var httpClient = HttpClient;
            var response = await httpClient.PostAsJsonAsync(FilterAllMovieConnectionsEndpoint, filters, GlobalSerializerOptions.Options);
            if (response.IsSuccessStatusCode)
            {
                movieConnections = await response.Content.ReadFromJsonAsync<MovieConnection.List>();               
            }

            return movieConnections;
        }

        public virtual async Task<MovieConnection.List> FilterMovieConnectionsForMovie(string title, int releaseYear, IEnumerable<IMovieConnectionListFilter> filters)
        {
            MovieConnection.List movieConnections = null;

            var httpClient = HttpClient;
            var uri = string.Format(FilterMovieConnectionsForMovieEndpointFormat, title, releaseYear);
            var response = await httpClient.PostAsJsonAsync(uri, filters, GlobalSerializerOptions.Options);
            if (response.IsSuccessStatusCode)
            {
                movieConnections = await response.Content.ReadFromJsonAsync<MovieConnection.List>();                
            }

            return movieConnections;
        }

        public async Task<MovieConnection> GetMovieConnection(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle,
            int targetMovieReleaseYear)
        {
            var httpClient = HttpClient;
            var uri = string.Format(GetMovieConnectionByMoviesEndpointFormat,
                                         sourceMovieTitle,
                                         sourceMovieReleaseYear,
                                         targetMovieTitle,
                                         targetMovieReleaseYear);
            return await httpClient.GetFromJsonAsync<MovieConnection>(uri);
        }

        public async Task<MovieConnection> GetMovieConnection(int movieConnectionId)
        {
            var httpClient = HttpClient;
            var uri = string.Format(GetMovieConnectionByIdEndpointFormat, movieConnectionId);
            return await httpClient.GetFromJsonAsync<MovieConnection>(uri);
        }

        public async Task<string> GetMovieConnectionsGraphForMovie(string title, int releaseYear)
        {
            var httpClient = HttpClient;
            var uri = string.Format(MovieConnectionsGraphForMovieEndpointFormat, title, releaseYear);
            var stream = await httpClient.GetStringAsync(uri);
            return stream;
        }

        public async Task<string> GetAllMovieConnectionsGraph()
        {
            var httpClient = HttpClient;
            var uri = string.Format(AllMovieConnectionsGraphEndpointFormat);
            var stream = await httpClient.GetStringAsync(uri);
            return stream;
        }
    }
}
