using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMDbLib.Objects.Movies;

namespace MovieMatchMakerLib
{
    public class MovieNetworkDataBuilder
    {
        private readonly IDataSource _dataSource;        

        public MovieNetworkDataBuilder(IDataSource dataSource)
        {
            _dataSource = dataSource;            
        }

        public async Task BuildFromInitial(string title, int releaseYear, int degree)
        {
            await FindMoviesConnectedToMovie(title, releaseYear, degree);
        }

        private async Task FindMoviesConnectedToMovie(string title, int releaseYear, int degree)
        {
            if (degree >= 0)
            {
                var sourceMovie = await _dataSource.GetMovieAsync(title, releaseYear);
                if (sourceMovie != null)
                {
                    // not from the cache (if so we already have its credits)
                    if (sourceMovie.Fetched)
                    {
                        await FindMoviesConnectedToMovie(sourceMovie.MovieId, degree);
                    }
                }
            }
        }

        private async Task FindMoviesConnectedToMovie(int movieId, int degree)
        {
            var moviesCredits = await _dataSource.GetCreditsForMovieAsync(movieId);
            foreach (var credit in moviesCredits.Credits.Cast)
            {
                await FindMoviesConnectedToPerson(credit.Id, degree);
            }
            foreach (var credit in moviesCredits.Credits.Crew)
            {
                await FindMoviesConnectedToPerson(credit.Id, degree);
            }
        }

        private async Task FindMoviesConnectedToPerson(int personId, int degree)
        {
            var personsMovieCredits = await _dataSource.GetMovieCreditsForPersonAsync(personId);
            foreach (var role in personsMovieCredits.MovieCredits.Cast)
            {
                if (role.ReleaseDate.HasValue)
                {
                    await FindMoviesConnectedToMovie(role.Title, role.ReleaseDate.Value.Year, degree - 1);
                    //var connectedMovie = await _dataSource.GetMovieAsync(role.Title, role.ReleaseDate.Value.Year);
                    //var connectedMoviesCredits = await _dataSource.GetCreditsForMovieAsync(connectedMovie.MovieId);
                }
            }
            foreach (var role in personsMovieCredits.MovieCredits.Crew)
            {
                if (role.ReleaseDate.HasValue)
                {
                    await FindMoviesConnectedToMovie(role.Title, role.ReleaseDate.Value.Year, degree - 1);
                    //var connectedMovie = await _dataSource.GetMovieAsync(role.Title, role.ReleaseDate.Value.Year);
                    //var connectedMoviesCredits = await _dataSource.GetCreditsForMovieAsync(connectedMovie.MovieId);
                }                              
            }
        }   
    }
}
