﻿using System;
using System.Linq;
using System.Threading.Tasks;

using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.TmdbApi;
using MovieMatchMakerLib.Utils;

using TMDbLib.Objects.People;

namespace MovieMatchMakerLib.Data
{
    public class ApiDataSource : IDataSource
    {
        private readonly ITmdbApi _tmdbApi;

        public int MoviesFetched { get; set; }
        public int MovieCreditsFetched { get; set; }
        public int PersonMoviesCreditsFetched { get; set; }

        public ApiDataSource()
        {
            _tmdbApi = new TmdbLibApi(TmdbApiHelper.TmdbApiKey);
        }

        public async Task<MoviesCredits> GetCreditsForMovieAsync(int movieId)
        {
            var movieCredits = await _tmdbApi.FetchMovieCreditsAsync(movieId);
            if (movieCredits != null)
            {                
                var moviesCredits = new MoviesCredits()
                {
                    MovieId = movieId,
                    Credits = movieCredits
                };
                MovieCreditsFetched++;
                return moviesCredits;
            }
            return null;
        }

        public async Task<Movie> GetMovieAsync(string title, int releaseYear)
        {
            var movie = await _tmdbApi.FetchMovieAsync(title, releaseYear);
            MoviesFetched++;
            return movie;
        }

        public async Task<PersonsMovieCredits> GetMovieCreditsForPersonAsync(int personId)
        {
            var movieCredits = await _tmdbApi.FetchMovieCreditsForPerson(personId);
            if (movieCredits != null)
            {                
                var profileImageData = GetPersonPosterPath(movieCredits);               
                var personsMovieCredits = new PersonsMovieCredits()
                {
                    PersonId = personId,
                    MovieCredits = movieCredits,
                    ProfileImagePath = profileImageData
                };
                MovieCreditsFetched++;
                return personsMovieCredits;
            }

            return null;
        }

        private static string GetPersonPosterPath(MovieCredits movieCredits)
        {
            var profileImageData = "";
            if (movieCredits.Cast.Count > 0)
            {
                profileImageData = movieCredits.Cast.First().PosterPath;
            }
            else if (movieCredits.Crew.Count > 0)
            {
                profileImageData = movieCredits.Crew.First().PosterPath;
            }
            return profileImageData;
        }
    }
}
