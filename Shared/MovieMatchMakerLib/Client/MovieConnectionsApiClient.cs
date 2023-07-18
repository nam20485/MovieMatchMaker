using System;
using System.Collections.Generic;
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
    public class MovieConnectionsApiClient : IMovieConnectionsClient
    {
        private readonly HttpClient _httpClient;

        public MovieConnectionsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MovieConnection.List> GetAllMovieConnections()
        {
            //try
            {
                return await _httpClient.GetFromJsonAsync<MovieConnection.List>("MovieConnections/movieconnections", MyJsonSerializerOptions.JsonSerializerOptions);
            }
            //catch (Exception ex)
            {
                //    Console.WriteLine(ex);
            }
        }

        public Task<MovieConnection.List> GetMovieConnectionsForMovie(string title, int releaseYear)
        {
            throw new NotImplementedException();
        }

        public Task<MovieConnection.List> FilterAllMovieConnections(IEnumerable<IMovieConnectionListFilter> filters)
        {
            throw new NotImplementedException();
        }

        public Task<MovieConnection.List> FilterMovieConnectionsForMovie(string title, int releaseYear, IEnumerable<IMovieConnectionListFilter> filters)
        {
            throw new NotImplementedException();
        }
    }
}
