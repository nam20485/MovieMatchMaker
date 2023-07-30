using System;
using System.IO;

using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public class ThreadedMovieDataBuilder : IMovieDataBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.LocalAppDataPath, "moviedata.json");

        private readonly IDataSource _dataSource;

        private readonly RequestProcessingLoopThread<MovieRequest> _movieRequestsLoopThread;
        private readonly RequestProcessingLoopThread<MovieCreditsRequest> _movieCreditsRequestsLoopThread;
        private readonly RequestProcessingLoopThread<PersonCreditsRequest> _personCreditsRequestsLoopThread;
       
        public ThreadedMovieDataBuilder(IDataSource dataSource)
        {
            _dataSource = dataSource;

            _movieRequestsLoopThread = new (ProcessMovieRequestAsync);
            _movieCreditsRequestsLoopThread = new (ProcessMovieCreditsRequestAsync);
            _personCreditsRequestsLoopThread = new (ProcessPersonCreditsRequestAsync);
        }    

        public void BuildFreshFromInitial(string title, int releaseYear, int degree)
        {            
            _movieRequestsLoopThread.AddRequest(new MovieRequest(title, releaseYear, degree));
            Start();
        }

        public void ContinueFromExisting(int degree)
        {
            // determine from the cache a list of movies to start fetching
            // movies in PersonsMoviesCredits that don't have corresponding Movies in the cache???
        }

        private void Start()
        {
            _movieRequestsLoopThread.StartProcessingRequests();
            _movieCreditsRequestsLoopThread.StartProcessingRequests();
            _personCreditsRequestsLoopThread.StartProcessingRequests();
        }

        public void Stop()
        {
            _movieRequestsLoopThread.StopProcessingRequests();
            _movieCreditsRequestsLoopThread.StopProcessingRequests();
            _personCreditsRequestsLoopThread.StopProcessingRequests();
        }

        public void Wait()
        {
            _movieRequestsLoopThread.Wait();
            _movieCreditsRequestsLoopThread.Wait();
            _personCreditsRequestsLoopThread.Wait();
        }

        protected virtual async void ProcessMovieRequestAsync(MovieRequest movieRequest)
        {
            if (movieRequest.Degree >= 0)
            {               
                var movie = await _dataSource.GetMovieAsync(movieRequest.Title, movieRequest.ReleaseYear);
                //if (movie is not null)
                {
                    if (movie.Fetched /*|| movieRequest.Degree > 0*/)
                    {
                        // if Fetched, then it wasn't from the cache. Otherwise it came from the cache and
                        // therefor it's already had its credits and its roles' credits fetched and filled out
                        _movieCreditsRequestsLoopThread.AddRequest(new MovieCreditsRequest(movie.MovieId, movieRequest.Degree));
                    }
                }
            }
        }

        protected virtual async void ProcessMovieCreditsRequestAsync(MovieCreditsRequest request)
        {
            // fetch credits            
            // add movie credits to cache
            var movieCredits = await _dataSource.GetCreditsForMovieAsync(request.MovieId);
            if (movieCredits is not null)
            {
                foreach (var castRole in movieCredits.Credits.Cast)
                {
                    _personCreditsRequestsLoopThread.AddRequest(new PersonCreditsRequest(castRole.Id, request.Degree));
                }
                foreach (var crewRole in movieCredits.Credits.Crew)
                {
                    _personCreditsRequestsLoopThread.AddRequest(new PersonCreditsRequest(crewRole.Id, request.Degree));
                }
            }
        }

        protected virtual async void ProcessPersonCreditsRequestAsync(PersonCreditsRequest request)
        {
            var personCredits = await _dataSource.GetMovieCreditsForPersonAsync(request.PersonId);
            if (personCredits is not null)
            {
                foreach (var castRole in personCredits.MovieCredits.Cast)
                {
                    if (castRole.ReleaseDate.HasValue)
                    {
                        _movieRequestsLoopThread.AddRequest(new MovieRequest(castRole.Title, castRole.ReleaseDate.Value.Year, request.Degree - 1));
                    }
                }
                foreach (var crewRole in personCredits.MovieCredits.Crew)
                {
                    if (crewRole.ReleaseDate.HasValue)
                    {
                        _movieRequestsLoopThread.AddRequest(new MovieRequest(crewRole.Title, crewRole.ReleaseDate.Value.Year, request.Degree - 1));
                    }
                }
            }
        }
       
        protected readonly struct MovieRequest
        {
            public readonly string Title;
            public readonly int ReleaseYear;
            public readonly int Degree;

            public MovieRequest(string title, int releaseYear, int degree)
            {
                Title = title;
                ReleaseYear = releaseYear;
                Degree = degree;
            }
        }

        protected readonly struct MovieCreditsRequest
        {
            public readonly int MovieId;
            public readonly int Degree;

            public MovieCreditsRequest(int movieId, int degree)
            {
                MovieId = movieId;
                Degree = degree;
            }
        }

        protected readonly struct PersonCreditsRequest
        {
            public readonly int PersonId;
            public readonly int Degree;

            public PersonCreditsRequest(int personId, int degree)
            {
                PersonId = personId;
                Degree = degree;
            }
        }       
    }
}
