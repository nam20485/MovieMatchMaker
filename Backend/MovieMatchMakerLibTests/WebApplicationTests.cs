using System.Net;

using Microsoft.AspNetCore.Mvc.Testing;

using MovieMatchMakerLib.Model;


namespace MovieMatchMakerLibTests
{
    // https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0#basic-tests-with-the-default-webapplicationfactory
    public class WebApplicationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WebApplicationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]        
        [InlineData("/api/MovieConnections/movieconnections", "application/json; charset=utf-8", HttpStatusCode.OK)]
        [InlineData("/api/MovieConnections/movieconnections/Dark%20City/1998", "application/json; charset=utf-8", HttpStatusCode.OK)]        
        [InlineData("/api/MovieConnections/movieconnections/graph/Dark%20City/1998", "text/plain; charset=utf-8", HttpStatusCode.OK)]
        [InlineData("/api/MovieConnections/movieconnections/graph", "text/plain; charset=utf-8", HttpStatusCode.OK)]
        [InlineData("/api/MovieConnections/movieconnection/0", "application/json; charset=utf-8", HttpStatusCode.OK)]
        //[InlineData("/api/MovieConnections/movieconnection/In%20a%20Savage%20Land/1999/Dark%20City/1998", "application/json; charset=utf-8", HttpStatusCode.NoContent)]
        public async Task Test_Get_EndpointsReturnSuccessAndCorrectContentType_Urls(string url, string contentType, HttpStatusCode statusCode)
        {            
            var client = _factory.CreateClient();         
            var response = await client.GetAsync(url);

            //response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(statusCode);
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().NotBeEmpty();
            response.Content.Headers.ContentType.ToString().Should().Be(contentType);          
        }

        [Theory]
        [InlineData("/swagger")]
        [InlineData("/swagger/index.html")]
        [InlineData("/api-docs")]
        [InlineData("/api-docs/index.html")]
        public async Task Test_Get_EndpointsReturnSuccessAndCorrectContentType_Swagger(string url)
        {            
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            //response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().NotBeEmpty();
            if (url == "/swagger" || url == "/swagger/index.html")
            {
                response.Content.Headers.ContentType.ToString().Should().Be("text/html; charset=utf-8");
            }
            else if (url == "/api-docs" || url == "/api-docs/index.html")
            {
                response.Content.Headers.ContentType.ToString().Should().StartWith("text/html");
            }
        }

        [Theory]
        [InlineData("/api/MovieConnections/movieconnections", 39)]
        [InlineData("/api/MovieConnections/movieconnections/Dark%20City/1998", 0)]
        public async Task Test_AllMovieConnections_Deserialize(string url, int expectedCount)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().NotBeEmpty();
            response.Content.Headers.ContentType.ToString().Should().StartWith("application/json");

            var strContent = await response.Content.ReadAsStringAsync();
            strContent.Should().NotBeNull();
            strContent.Should().NotBeEmpty();
            var movieConnections = MovieConnection.List.FromJson(strContent);                
            movieConnections.Should().NotBeNull();
            movieConnections.Should().HaveCount(expectedCount);
        }
    }
}
