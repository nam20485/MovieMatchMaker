using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib
{
    internal class DbCache : IDataCache
    {
        public async Task AddMovieAsync(Movie movie)
        {
            using (var dbContext = new MovieMatchMakerContext())
            {
                dbContext.Movies.Add(movie);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddPersonsMovieCreditsAsync(PersonsMovieCredits personsMovieCredits)
        {
            using (var dbContext = new MovieMatchMakerContext())
            {
                await dbContext.PersonsMovieCredits.AddAsync(personsMovieCredits);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Movie> GetMovieAsync(string title, int releaseYear)
        {
            Movie movie = null;
            using (var dbContext = new MovieMatchMakerContext())
            {
                movie = await dbContext.Movies.AsNoTracking().SingleOrDefaultAsync(m => m.Title == title &&
                                                                                        m.ReleaseYear == releaseYear);
            }

            return movie;
        }

        public async Task<MoviesCredits> GetCreditsForMovieAsync(int movieId)
        {
            using (var dbContext = new MovieMatchMakerContext())
            {
                return await dbContext
                                .MoviesCredits
                                .AsNoTracking()
                                .SingleOrDefaultAsync(mc => mc.MovieId == movieId);
            }
        }

        public async Task<PersonsMovieCredits> GetMovieCreditsForPersonAsync(int personId)
        {
            using (var dbContext = new MovieMatchMakerContext())
            {
                return await dbContext
                                .PersonsMovieCredits
                                .AsNoTracking()
                                .SingleOrDefaultAsync(pmc => pmc.PersonId == personId);
            }
        }

        public Task UpdateMovieCreditsAsync(string title, int releaseYear)
        {
            throw new NotImplementedException();
        }

        public async Task AddCreditsForMovieAsync(MoviesCredits moviesCredits)
        {
            using (var dbContext = new MovieMatchMakerContext())
            {
                await dbContext.MoviesCredits.AddAsync(moviesCredits);
                await dbContext.SaveChangesAsync();
            }
        }

        public void SaveAsync()
        {
            throw new NotImplementedException();
        }

        public void AddCreditsForMovie(MoviesCredits moviesCredits)
        {
            throw new NotImplementedException();
        }

        public void AddMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public void AddPersonsMovieCredits(PersonsMovieCredits personsMovieCredits)
        {
            throw new NotImplementedException();
        }

        Task IDataCache.SaveAsync()
        {
            throw new NotImplementedException();
        }

        //Task IDataCache.SaveAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
