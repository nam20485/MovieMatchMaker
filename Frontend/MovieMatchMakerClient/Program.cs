using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MovieMatchMakerClient;
using MovieMatchMakerLib.Client;
using MudBlazor.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// MudBlazor
builder.Services.AddMudServices();

builder.Services.AddHttpClient<IMovieConnectionsClient, MovieConnectionsApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7288/api/");
});

await builder.Build().RunAsync();
