﻿using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;

namespace MovieMatchMakerApi.Services
{
    public class MovieDataBuilderService : IMovieDataBuilderService
    {
        public MovieDataBuilder MovieDataBuilder { get; }

        private readonly ILogger<MovieDataBuilderService> _logger;

        public MovieDataBuilderService(ILogger<MovieDataBuilderService> logger)
        {
            _logger = logger;

            var apiDataSource = new ApiDataSource();
            var dataCache = JsonFileCache.Load(MovieDataBuilderBase.FilePath);
            var cachedDataSource = new CachedDataSource(dataCache, apiDataSource);

            MovieDataBuilder = new MovieDataBuilder(cachedDataSource);
        }
    }
}
