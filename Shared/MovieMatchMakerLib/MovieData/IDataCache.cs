using System;
using System.Threading.Tasks;

using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Data
{
    public interface IDataCache : IDataSource, IDisposable
    {
        //void Load();
        Task SaveAsync();

        Task AddCreditsForMovieAsync(MoviesCredits moviesCredits);
        Task AddMovieAsync(Movie movie);
        Task AddPersonsMovieCreditsAsync(PersonsMovieCredits personsMovieCredits);

        void AddCreditsForMovie(MoviesCredits moviesCredits);
        void AddMovie(Movie movie);
        void AddPersonsMovieCredits(PersonsMovieCredits personsMovieCredits);

        Movie.List Movies { get; }
        MoviesCredits.IntDictionary MoviesCreditsById { get; }
        PersonsMovieCredits.IntDictionary PersonsMovieCreditsById { get; }
     
        Movie GetMovie(int movieId);
        void Save();
        void Clear();
        void Start();

        int MovieCount { get; }
        int MovieCreditsCount { get; }
        int PersonMoviesCreditsCount { get; }
    }
}