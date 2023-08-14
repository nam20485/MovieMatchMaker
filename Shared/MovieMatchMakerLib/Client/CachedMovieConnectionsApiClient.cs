using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Client;

public class CachedMovieConnectionsApiClient : MovieConnectionsApiClient
{
    private MovieConnection.List _movieConnections;

    private readonly ILogger<CachedMovieConnectionsApiClient> _logger;
    
    public CachedMovieConnectionsApiClient(IHttpClientFactory httpClient, ILogger<CachedMovieConnectionsApiClient> logger)
        : base(httpClient, logger)
    {
        _movieConnections = null;
    }
    
    public override async Task<MovieConnection.List> GetAllMovieConnections()
    {
        return _movieConnections ??= await base.GetAllMovieConnections();        
    }

    public override async Task<MovieConnection.List> GetMovieConnectionsForMovie(string title, int releaseYear)
    {
        var connections = await GetAllMovieConnections();
        if (connections is not null)
        {
            return connections.FindForMovie(title, releaseYear);
        }
        return MovieConnection.List.Empty;
    }

    public override async Task<MovieConnection.List> FilterAllMovieConnections(IEnumerable<IMovieConnectionListFilter> filters)
    {
        var connections = await GetAllMovieConnections();
        if (connections is not null)
        {
            return connections.Filter(filters);
        }
        return MovieConnection.List.Empty;
    }

    public override async Task<MovieConnection.List> FilterMovieConnectionsForMovie(string title, int releaseYear, IEnumerable<IMovieConnectionListFilter> filters)
    {
        var connections = await GetAllMovieConnections();
        if (connections is not null)
        {
            return connections.FindForMovie(title, releaseYear).Filter(filters);
        }

        return MovieConnection.List.Empty;
    }
}