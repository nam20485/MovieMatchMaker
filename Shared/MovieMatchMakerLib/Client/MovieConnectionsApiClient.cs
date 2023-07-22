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

        private readonly HttpClient _httpClient;          

        public MovieConnectionsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public virtual async Task<MovieConnection.List> GetAllMovieConnections()
        {            
            return await _httpClient.GetFromJsonAsync<MovieConnection.List>(AllMovieConnectionsEndpoint, MyJsonSerializerOptions.JsonSerializerOptions);
        }

        public virtual async Task<MovieConnection.List> GetMovieConnectionsForMovie(string title, int releaseYear)
        {
            var uri = string.Format(MovieConnectionsForMovieEndpointFormat, title, releaseYear);
            return await _httpClient.GetFromJsonAsync<MovieConnection.List>(uri, MyJsonSerializerOptions.JsonSerializerOptions);
        }

        public virtual async Task<MovieConnection.List> FilterAllMovieConnections(IEnumerable<IMovieConnectionListFilter> filters)
        {
            MovieConnection.List movieConnections = null;

            var response = await _httpClient.PostAsJsonAsync<IEnumerable<IMovieConnectionListFilter>>(FilterAllMovieConnectionsEndpoint, filters, MyJsonSerializerOptions.JsonSerializerOptions);
            if (response.IsSuccessStatusCode)
            {
                movieConnections = await response.Content.ReadFromJsonAsync<MovieConnection.List>();               
            }
            return movieConnections;
        }

        public virtual async Task<MovieConnection.List> FilterMovieConnectionsForMovie(string title, int releaseYear, IEnumerable<IMovieConnectionListFilter> filters)
        {
            MovieConnection.List movieConnections = null;

            var uri = string.Format(FilterMovieConnectionsForMovieEndpointFormat, title, releaseYear);
            var response = await _httpClient.PostAsJsonAsync<IEnumerable<IMovieConnectionListFilter>>(uri, filters, MyJsonSerializerOptions.JsonSerializerOptions);
            if (response.IsSuccessStatusCode)
            {
                movieConnections = await response.Content.ReadFromJsonAsync<MovieConnection.List>();                
            }

            return movieConnections;
        }

        public async Task<MovieConnection> GetMovieConnection(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle,
            int targetMovieReleaseYear)
        {
            var uri = string.Format(GetMovieConnectionByMoviesEndpointFormat,
                                         sourceMovieTitle,
                                         sourceMovieReleaseYear,
                                         targetMovieTitle,
                                         targetMovieReleaseYear);
            return await _httpClient.GetFromJsonAsync<MovieConnection>(uri);
        }

        public async Task<MovieConnection> GetMovieConnection(int movieConnectionId)
        {
            var uri = string.Format(GetMovieConnectionByIdEndpointFormat, movieConnectionId);
            return await _httpClient.GetFromJsonAsync<MovieConnection>(uri);
        }

        public async Task<Stream> GetMovieConnectionsGraphForMovie(string title, int releaseYear)
        {
            var uri = string.Format(MovieConnectionsGraphForMovieEndpointFormat, title, releaseYear);
            var stream = await _httpClient.GetStreamAsync(uri);
            return stream;
        }
    }
}
