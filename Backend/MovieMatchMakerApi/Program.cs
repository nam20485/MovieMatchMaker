using Microsoft.AspNetCore.ResponseCompression;
using MovieMatchMakerApi;
using MovieMatchMakerApi.Services;
using MovieMatchMakerLib;
using MovieMatchMakerLib.Utils;


const bool UseResponseCompression = true;

var builder = WebApplication.CreateBuilder(args);

//
//  Add services to the container.
//
if (UseResponseCompression)
{
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[]
            {
                "image/svg+xml"
            });
    });
}

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.SetFrom(GlobalSerializerOptions.Options);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

//  My custom services
builder.Services.AddSingleton<IMovieConnectionsService, MovieConnectionsService>();
//builder.Services.AddSingleton<IMovieConnectionBuilderService, MovieConnectionBuilderService>();
//builder.Services.AddSingleton<IMovieDataBuilderService, MovieDataBuilderService>();

//
//  Configure the app 
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.ConfigureSwaggerUI();
    app.UseReDoc();

    app.UseCors(configure =>
    {
        configure
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
}
else
{
    app.UseHttpsRedirection();
}

if (UseResponseCompression)
{
    app.UseResponseCompression();
}

app.UseHttpLogging();
app.UseAuthorization();
app.MapControllers();

//
//  Run the app
//
app.Run();

// make Program class public for access from test project
public partial class Program { }
