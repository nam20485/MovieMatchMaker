using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib.Data;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public class ThreadedMovieDataBuilder
    {
        public static string FilePath => Path.Combine(SystemFolders.LocalAppDataPath, "moviedata.json");

        private readonly IDataCache _dataCahe;

        private readonly ConcurrentQueue<MovieRequest> _movieRequests;
        private readonly ConcurrentQueue<MovieCreditsRequest> _movieCreditsRequests;
        private readonly ConcurrentQueue<PersonCreditsRequest> _personCreditsRequests;

        public ThreadedMovieDataBuilder()
        {
            _dataCahe = new JsonFileCache();
        }

        private struct MovieRequest
        {
            readonly string title;
            readonly int releaseYear;
        }

        private struct MovieCreditsRequest
        {
            readonly int movieId;
        }

        private struct PersonCreditsRequest
        {
            readonly int personId;
        }
    }
}
