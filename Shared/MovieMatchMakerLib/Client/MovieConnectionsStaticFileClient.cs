using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib.Client
{
    public class MovieConnectionsStaticFileClient : IMovieConnectionsClient
    {
        public const string MovieConnectionsFilename = "movieconnections.json";       

        private readonly IHttpClientFactory _httpClientFactory;

        private MovieConnection.List _movieConnections;

        public MovieConnectionsStaticFileClient(IHttpClientFactory httpClientFactory)
        {
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
                using var httpClient = _httpClientFactory.CreateClient("Static");
                _movieConnections = await httpClient.GetFromJsonAsync<MovieConnection.List>(MovieConnectionsFilename, MyJsonSerializerOptions.JsonSerializerOptions);
            }
            return _movieConnections;
        }
    }
}
