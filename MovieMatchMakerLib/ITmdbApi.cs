using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib
{
    public interface ITmdbApi
    {       
        Task<Movie> FetchMovieAsync(string title, int releaseYear);                
        Task<Credits> FetchMovieCreditsAsync(string title, int releaseYear);
        Task<Credits> FetchMovieCreditsAsync(int movieApiId);
        Task<MovieCredits> FetchMovieCreditsForPerson(int personApiId);
        //Task<Movie> FetchConnectedMovies(string title, int releaseYear);
    }
}
