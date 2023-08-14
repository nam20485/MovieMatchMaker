using MovieMatchMakerApi;
using MovieMatchMakerApi.Services;
using MovieMatchMakerLib;
using MovieMatchMakerLib.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.SetFrom(GlobalSerializerOptions.Options);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

// my services
builder.Services.AddSingleton<IMovieConnectionsService, MovieConnectionsService>();
//builder.Services.AddSingleton<IMovieConnectionBuilderService, MovieConnectionBuilderService>();
//builder.Services.AddSingleton<IMovieDataBuilderService, MovieDataBuilderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.ConfigureSwaggerUI();
    app.UseReDoc();

    app.UseCors(configure =>
    {
        configure.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseHttpLogging();

app.UseAuthorization();
app.MapControllers();

app.Run();

// make Program class public for access from test project
public partial class Program { }
