using MovieMatchMakerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// my services
builder.Services.AddSingleton<IMovieConnectionsService, MovieConnectionsService>();
builder.Services.AddSingleton<IMovieConnectionBuilderService, MovieConnectionBuilderService>();
builder.Services.AddSingleton<IMovieDataBuilderService, MovieDataBuilderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

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
