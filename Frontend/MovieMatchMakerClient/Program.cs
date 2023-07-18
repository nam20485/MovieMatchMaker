using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MovieMatchMakerClient;
using MovieMatchMakerLib.Client;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// configure logging from appsettings.json
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// MudBlazor
builder.Services.AddMudServices();

var apiUrl = builder.Configuration["ApiUrl"];
//if (string.IsNullOrWhiteSpace(apiUrl))
//{
//    // hard-coded default
//    apiUrl = "https://localhost:7288/api/";
//}

builder.Services.AddHttpClient<IMovieConnectionsClient, MovieConnectionsApiClient>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});

await builder.Build().RunAsync();
