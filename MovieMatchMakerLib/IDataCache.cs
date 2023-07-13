using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MovieMatchMakerLib
{
    public interface IDataCache : IDataSource
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
    }
}