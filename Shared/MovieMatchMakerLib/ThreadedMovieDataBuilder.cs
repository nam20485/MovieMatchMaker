using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public class ThreadedMovieDataBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.LocalAppDataPath, "moviedata.json");

        private readonly IDataSource _dataSource;

        private readonly RequestProcessingLoopThread<MovieRequest> _movieRequestsLoopThread;
        private readonly RequestProcessingLoopThread<MovieCreditsRequest> _movieCreditsRequestsLoopThread;
        private readonly RequestProcessingLoopThread<PersonCreditsRequest> _personCreditsRequestsLoopThread;


        public ThreadedMovieDataBuilder(string filePath)
        {
            _dataSource = CachedDataSource.Create(filePath);
           
        }

        public ThreadedMovieDataBuilder()
            : this(FilePath)
        {
            _movieRequestsLoopThread = new (ProcessMovieRequestAsync);
            _movieCreditsRequestsLoopThread = new (ProcessMovieCreditsRequestAsync);
            _personCreditsRequestsLoopThread = new (ProcessPersonCreditsRequestAsync);
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
            var movie = await _dataSource.GetMovieAsync(movieRequest.Title, movieRequest.ReleaseYear);
            _movieCreditsRequestsLoopThread.AddRequest(new MovieCreditsRequest(movie.MovieId));
        }
      
        private async void ProcessMovieCreditsRequestAsync(MovieCreditsRequest request)
        {
            // fetch credits            
            // add movie credits to cache
            var movieCredits = await _dataSource.GetCreditsForMovieAsync(request.MovieId);
            foreach (var castRole in movieCredits.Credits.Cast)
            {
                _personCreditsRequestsLoopThread.AddRequest(new PersonCreditsRequest(castRole.Id));
            }
            foreach (var crewRole in movieCredits.Credits.Crew)
            {
                _personCreditsRequestsLoopThread.AddRequest(new PersonCreditsRequest(crewRole.Id));
            }
        }

        private async void ProcessPersonCreditsRequestAsync(PersonCreditsRequest request)
        {
            var personCredits = await _dataSource.GetMovieCreditsForPersonAsync(request.PersonId);
            foreach (var castRole in personCredits.MovieCredits.Cast)
            {
                _movieRequestsLoopThread.AddRequest(new MovieRequest(castRole.Title, castRole.ReleaseDate.Value.Year));
            }
            foreach (var crewRole in personCredits.MovieCredits.Crew)
            {
                _movieRequestsLoopThread.AddRequest(new MovieRequest(crewRole.Title, crewRole.ReleaseDate.Value.Year));
            }
        }


        private readonly struct MovieRequest
        {
            public readonly string Title;
            public readonly int ReleaseYear;

            public MovieRequest(string title, int releaseYear)
            {
                Title = title;
                ReleaseYear = releaseYear;
            }
        }

        private readonly struct MovieCreditsRequest
        {
            public readonly int MovieId;

            public MovieCreditsRequest(int movieId)
            {
                MovieId = movieId;
            }
        }

        private readonly struct PersonCreditsRequest
        {
            public readonly int PersonId;

            public PersonCreditsRequest(int personId)
            {
                PersonId = personId;
            }
        }       
    }
}
