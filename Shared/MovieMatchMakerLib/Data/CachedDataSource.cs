using System.Threading.Tasks;

using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Data
{
    public class CachedDataSource : IDataSource
    {
        private readonly IDataCache _dataCache;
        private readonly IDataSource _dataSource;

        public CachedDataSource(IDataCache dataCache, IDataSource dataSource)
        {
            _dataCache = dataCache;
            _dataSource = dataSource;
        }

        public async Task<Movie> GetMovieAsync(string title, int releaseYear)
        {
            var movie = await _dataCache.GetMovieAsync(title, releaseYear);
            if (movie is null)
            {
                movie = await _dataSource.GetMovieAsync(title, releaseYear);
                if (movie != null)
                {
                    // TODO: does this work? (won't movies be stored int he cache as Fetched = true?)
                    movie.Fetched = true;
                    await _dataCache.AddMovieAsync(movie);
                }
            }
            else
            {
                movie.Fetched = false;
            }

            return movie;
        }

        //public async Task UpdateMovieCreditsAsync(string title, int releaseYear)
        //{
        //    Movie movie = null;
        //    using (var dbContext = new MovieMatchMakerContext())
        //    {
        //        movie = await dbContext.Movies.SingleOrDefaultAsync(m => m.Title == title &&
        //                                                            m.ReleaseYear == releaseYear);                
        //    }

        //    if (movie != null)
        //    {
        //        var credits = await GetCreditsForMovieAsync(movie.MovieId);
        //        if (credits != null)
        //        {
        //            if (movie.Members.Count == 0)
        //            {
        //                foreach (var crew in credits.Crew)
        //                {
        //                    movie.Members.Add(new ProductionMember
        //                    {
        //                        Job = crew.Job,
        //                        Name = new Name(crew.Name),
        //                        MemberType = ProductionMember.Type.Crew,
        //                        ApiId = crew.Id
        //                    });
        //                }
        //                foreach (var cast in credits.Cast)
        //                {
        //                    movie.Members.Add(new ProductionMember
        //                    {
        //                        Job = cast.Character,
        //                        Name = new Name(cast.Name),
        //                        MemberType = ProductionMember.Type.Cast,
        //                        ApiId = cast.Id
        //                    });
        //                }
        //            }
        //        }
        //    }
        //}     

        public async Task<PersonsMovieCredits> GetMovieCreditsForPersonAsync(int personId)
        {
            var personsMovieCredits = await _dataCache.GetMovieCreditsForPersonAsync(personId);
            if (personsMovieCredits is null)
            {
                personsMovieCredits = await _dataSource.GetMovieCreditsForPersonAsync(personId);
                if (personsMovieCredits != null)
                {
                    await _dataCache.AddPersonsMovieCreditsAsync(personsMovieCredits);
                }
            }

            return personsMovieCredits;
        }

        public async Task<MoviesCredits> GetCreditsForMovieAsync(int movieId)
        {
            var moviesCredits = await _dataCache.GetCreditsForMovieAsync(movieId);
            if (moviesCredits is null)
            {
                moviesCredits = await _dataSource.GetCreditsForMovieAsync(movieId);
                if (moviesCredits != null)
                {
                    await _dataCache.AddCreditsForMovieAsync(moviesCredits);
                }
            }

            return moviesCredits;
        }
    }
}