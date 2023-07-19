using System;
using System.Collections.Generic;
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
        public const string AllMovieConnectionsEndpoint = "MovieConnections/movieconnections";
        public const string MovieConnectionsForMovieEndpointFormat = "MovieConnections/movieconnections/{0}/{1}";        
        public const string FilterAllMovieConnectionsEndpoint = "MovieConnections/movieconnections/filter";
        public const string FilterMovieConnectionsForMovieEndpointFormat = "MovieConnections/movieconnections/filter/{0}/{1}";

        private readonly HttpClient _httpClient;          

        public MovieConnectionsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MovieConnection.List> GetAllMovieConnections()
        {            
            return await _httpClient.GetFromJsonAsync<MovieConnection.List>(AllMovieConnectionsEndpoint, MyJsonSerializerOptions.JsonSerializerOptions);
        }

        public async Task<MovieConnection.List> GetMovieConnectionsForMovie(string title, int releaseYear)
        {
            var uri = string.Format(MovieConnectionsForMovieEndpointFormat, title, releaseYear);
            return await _httpClient.GetFromJsonAsync<MovieConnection.List>(uri, MyJsonSerializerOptions.JsonSerializerOptions);
        }

        public async Task<MovieConnection.List> FilterAllMovieConnections(IEnumerable<IMovieConnectionListFilter> filters)
        {
            MovieConnection.List movieConnections = null;

            var response = await _httpClient.PostAsJsonAsync<IEnumerable<IMovieConnectionListFilter>>(FilterAllMovieConnectionsEndpoint, filters, MyJsonSerializerOptions.JsonSerializerOptions);
            if (response.IsSuccessStatusCode)
            {
                movieConnections = await response.Content.ReadFromJsonAsync<MovieConnection.List>();               
            }
            return movieConnections;
        }

        public async Task<MovieConnection.List> FilterMovieConnectionsForMovie(string title, int releaseYear, IEnumerable<IMovieConnectionListFilter> filters)
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
    }
}
