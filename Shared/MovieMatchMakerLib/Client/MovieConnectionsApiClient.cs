using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;

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

        private readonly IHttpClientFactory _httpClientFactory;          

        public MovieConnectionsApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public virtual async Task<MovieConnection.List> GetAllMovieConnections()
        {
            using var httpClient = _httpClientFactory.CreateClient("Api");
            return await httpClient.GetFromJsonAsync<MovieConnection.List>(AllMovieConnectionsEndpoint, GlobalSerializerOptions.JsonSerializerOptions);
        }

        public virtual async Task<MovieConnection.List> GetMovieConnectionsForMovie(string title, int releaseYear)
        {            
            using var httpClient = _httpClientFactory.CreateClient("Api");
            var uri = string.Format(MovieConnectionsForMovieEndpointFormat, title, releaseYear);
            return await httpClient.GetFromJsonAsync<MovieConnection.List>(uri, GlobalSerializerOptions.JsonSerializerOptions);
        }

        public virtual async Task<MovieConnection.List> FilterAllMovieConnections(IEnumerable<IMovieConnectionListFilter> filters)
        {
            MovieConnection.List movieConnections = null;

            using var httpClient = _httpClientFactory.CreateClient("Api");
            var response = await httpClient.PostAsJsonAsync<IEnumerable<IMovieConnectionListFilter>>(FilterAllMovieConnectionsEndpoint, filters, GlobalSerializerOptions.JsonSerializerOptions);
            if (response.IsSuccessStatusCode)
            {
                movieConnections = await response.Content.ReadFromJsonAsync<MovieConnection.List>();               
            }

            return movieConnections;
        }

        public virtual async Task<MovieConnection.List> FilterMovieConnectionsForMovie(string title, int releaseYear, IEnumerable<IMovieConnectionListFilter> filters)
        {
            MovieConnection.List movieConnections = null;

            var httpClient = _httpClientFactory.CreateClient("Api");
            var uri = string.Format(FilterMovieConnectionsForMovieEndpointFormat, title, releaseYear);
            var response = await httpClient.PostAsJsonAsync<IEnumerable<IMovieConnectionListFilter>>(uri, filters, GlobalSerializerOptions.JsonSerializerOptions);
            if (response.IsSuccessStatusCode)
            {
                movieConnections = await response.Content.ReadFromJsonAsync<MovieConnection.List>();                
            }

            return movieConnections;
        }

        public async Task<MovieConnection> GetMovieConnection(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle,
            int targetMovieReleaseYear)
        {
            var httpClient = _httpClientFactory.CreateClient("Api");
            var uri = string.Format(GetMovieConnectionByMoviesEndpointFormat,
                                         sourceMovieTitle,
                                         sourceMovieReleaseYear,
                                         targetMovieTitle,
                                         targetMovieReleaseYear);
            return await httpClient.GetFromJsonAsync<MovieConnection>(uri);
        }

        public async Task<MovieConnection> GetMovieConnection(int movieConnectionId)
        {
            var httpClient = _httpClientFactory.CreateClient("Api");
            var uri = string.Format(GetMovieConnectionByIdEndpointFormat, movieConnectionId);
            return await httpClient.GetFromJsonAsync<MovieConnection>(uri);
        }

        public async Task<Stream> GetMovieConnectionsGraphForMovie(string title, int releaseYear)
        {
            var httpClient = _httpClientFactory.CreateClient("Api");
            var uri = string.Format(MovieConnectionsGraphForMovieEndpointFormat, title, releaseYear);
            var stream = await httpClient.GetStreamAsync(uri);
            return stream;
        }
    }
}
