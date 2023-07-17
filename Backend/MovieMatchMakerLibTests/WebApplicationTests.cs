using Microsoft.AspNetCore.Mvc.Testing;


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
        [InlineData("/api/MovieConnections/movieconnections")]
        [InlineData("/api/MovieConnections/movieconnections/Dark%20City/1998")]
        public async Task Test_Get_EndpointsReturnSuccessAndCorrectContentType_Urls(string url)
        {            
            var client = _factory.CreateClient();         
            var response = await client.GetAsync(url);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().NotBeEmpty();
            response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");          
        }

        [Theory]
        [InlineData("/swagger")]
        [InlineData("/swagger/index.html")]       
        public async Task Test_Get_EndpointsReturnSuccessAndCorrectContentType_Swagger(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().NotBeEmpty();
            response.Content.Headers.ContentType.ToString().Should().Be("text/html; charset=utf-8");
        }
    }
}
