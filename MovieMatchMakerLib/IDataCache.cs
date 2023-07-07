using System.Threading.Tasks;

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
    }
}