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

        public void BuildFromInitial(string title, int releaseYear, int degree)
        {            
            _movieRequestsLoopThread.AddRequest(new MovieRequest(title, releaseYear, degree));
            Start();
        }

        private void Start()
        {
            _movieRequestsLoopThread.StartProcessingRequests();
            _movieCreditsRequestsLoopThread.StartProcessingRequests();
            _personCreditsRequestsLoopThread.StartProcessingRequests();
        }

        private void Stop()
        {
            _movieRequestsLoopThread.StopProcessingRequests();
            _movieCreditsRequestsLoopThread.StopProcessingRequests();
            _personCreditsRequestsLoopThread.StopProcessingRequests();
        }

        private async void ProcessMovieRequestAsync(MovieRequest movieRequest)
        {
            if (movieRequest.Degree >= 0)
            {
                var movie = await _dataSource.GetMovieAsync(movieRequest.Title, movieRequest.ReleaseYear);
                _movieCreditsRequestsLoopThread.AddRequest(new MovieCreditsRequest(movie.MovieId, movieRequest.Degree));
            }
        }
      
        private async void ProcessMovieCreditsRequestAsync(MovieCreditsRequest request)
        {
            // fetch credits            
            // add movie credits to cache
            var movieCredits = await _dataSource.GetCreditsForMovieAsync(request.MovieId);
            foreach (var castRole in movieCredits.Credits.Cast)
            {
                _personCreditsRequestsLoopThread.AddRequest(new PersonCreditsRequest(castRole.Id, request.Degree));
            }
            foreach (var crewRole in movieCredits.Credits.Crew)
            {
                _personCreditsRequestsLoopThread.AddRequest(new PersonCreditsRequest(crewRole.Id, request.Degree));
            }
        }

        private async void ProcessPersonCreditsRequestAsync(PersonCreditsRequest request)
        {
            var personCredits = await _dataSource.GetMovieCreditsForPersonAsync(request.PersonId);
            foreach (var castRole in personCredits.MovieCredits.Cast)
            {
                _movieRequestsLoopThread.AddRequest(new MovieRequest(castRole.Title, castRole.ReleaseDate.Value.Year, request.Degree - 1));
            }
            foreach (var crewRole in personCredits.MovieCredits.Crew)
            {
                _movieRequestsLoopThread.AddRequest(new MovieRequest(crewRole.Title, crewRole.ReleaseDate.Value.Year, request.Degree - 1));
            }
        }
       
        private readonly struct MovieRequest
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

        private readonly struct MovieCreditsRequest
        {
            public readonly int MovieId;
            public readonly int Degree;

            public MovieCreditsRequest(int movieId, int degree)
            {
                MovieId = movieId;
                Degree = degree;
            }
        }

        private readonly struct PersonCreditsRequest
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
