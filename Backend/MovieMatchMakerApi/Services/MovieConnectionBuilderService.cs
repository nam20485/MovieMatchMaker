﻿using MovieMatchMakerLib;
using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerApi.Services
{
    public class MovieConnectionBuilderService : IMovieConnectionBuilderService
    {
        public MovieConnectionBuilder MovieConnectionBuilder { get; }

        private readonly ILogger<MovieConnectionBuilderService> _logger;        

        public MovieConnectionBuilderService(ILogger<MovieConnectionBuilderService> logger)
        {
            _logger = logger;

            var dataCache = JsonFileCache.Load(MovieDataBuilderBase.FilePath);                        
            MovieConnectionBuilder = new MovieConnectionBuilder(dataCache);            
        }
    }
}
