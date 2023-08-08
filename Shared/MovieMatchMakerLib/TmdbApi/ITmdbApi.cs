using System;
using System.Threading.Tasks;

using MovieMatchMakerLib.Model;

using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib.TmdbApi
{
    public interface ITmdbApi : IDisposable
    {             
        Task<Model.Movie> FetchMovieAsync(string title, int releaseYear);
        //Task<Credits> FetchMovieCreditsAsync(string title, int releaseYear);
        Task<Credits> FetchMovieCreditsAsync(int movieApiId);
        Task<MovieCredits> FetchMovieCreditsForPerson(int personApiId);
        Task<ProfileImages> FetchImageDataForPerson(int personId);
        Task<TvShow> FetchTvShowAsync(string name);
        Task<TMDbLib.Objects.TvShows.Credits> FetchTvShowCreditsAsync(int tvShowId);
        //Task<Credits> FetchTvShowCreditsAsync(string title);
    }
}
