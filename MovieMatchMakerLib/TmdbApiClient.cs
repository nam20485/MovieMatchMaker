using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Microsoft.AspNetCore.WebUtilities;

namespace MovieMatchMakerLib
{
    public class TmdbApiClient : HttpClient
    {
        private readonly string ApiToken;
        private const string SearchMovieUrl = "https://api.themoviedb.org/3/search/movie";
        //private const string DiscoverUrl = "";

        public TmdbApiClient(string apiToken)
        {
            ApiToken = apiToken;
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> SearchMovieAsync(string title, int releaseYear)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["query"] = title,
                ["primary_release_year"] = releaseYear.ToString(),
            };
            var url = new Uri(QueryHelpers.AddQueryString(SearchMovieUrl, queryParams));
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }
    }
}
