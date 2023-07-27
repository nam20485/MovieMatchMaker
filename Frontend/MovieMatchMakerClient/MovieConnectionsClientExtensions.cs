using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace MovieMatchMakerLib.Client
{
    public static class MovieConnectionsClientExtensions
    {
        public static WebAssemblyHostBuilder AddMovieConnectionsClient(this WebAssemblyHostBuilder builder)
        {
            AddMovieConnectionsClientToBuilder(builder);
            return builder;
        }

        private static void AddMovieConnectionsClientToBuilder(WebAssemblyHostBuilder builder)
        {
            // DataSource (default to RemoteApi)
            var dataSource = IMovieConnectionsClient.DataSource.RemoteApi;
            if (builder.Configuration["DataSource"] is string strDataSource &&
                !string.IsNullOrWhiteSpace(strDataSource))
            {
                dataSource = Enum.Parse<IMovieConnectionsClient.DataSource>(strDataSource);
            }

            switch (dataSource)
            {
                case IMovieConnectionsClient.DataSource.RemoteApiCached:
                    if (builder.Configuration["ApiUrl"] is string apiUrl &&
                        Uri.IsWellFormedUriString(apiUrl, UriKind.RelativeOrAbsolute))
                    {
                        builder.Services.AddHttpClient("Api", client =>
                        {
                            client.BaseAddress = new Uri(apiUrl);
                        });
                        builder.Services.AddSingleton<IMovieConnectionsClient, CachedMovieConnectionsApiClient>();                       
                    }
                    break;
                case IMovieConnectionsClient.DataSource.RemoteApi:
                    // API client
                    if (builder.Configuration["ApiUrl"] is string apiUrl2 &&
                        Uri.IsWellFormedUriString(apiUrl2, UriKind.RelativeOrAbsolute))
                    {
                        builder.Services.AddHttpClient("Api", client =>
                        {
                            client.BaseAddress = new Uri(apiUrl2);
                        });
                        builder.Services.AddSingleton<IMovieConnectionsClient, MovieConnectionsApiClient>();
                    }
                    break;
                case IMovieConnectionsClient.DataSource.LocalFile:
                    // local static file client
                    builder.Services.AddHttpClient("Static", client =>
                    {
                        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                    });
                    builder.Services.AddScoped<IMovieConnectionsClient, MovieConnectionsStaticFileClient>();
                    break;
                case IMovieConnectionsClient.DataSource.LocalFileCached:
                    builder.Services.AddHttpClient("Static", client =>
                    {
                        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                    });
                    builder.Services.AddSingleton<IMovieConnectionsClient, MovieConnectionsStaticFileClient>();
                    break;
            }
        }
    }
}
