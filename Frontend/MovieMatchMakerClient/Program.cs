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

// MudBlazor
builder.Services.AddMudServices();

// add client
builder.AddMovieConnectionsClient();

await builder.Build().RunAsync();