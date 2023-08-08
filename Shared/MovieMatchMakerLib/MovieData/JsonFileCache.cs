using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;


namespace MovieMatchMakerLib.Data
{
    public class JsonFileCache : IDataCache, IDisposable
    {
        public string FilePath { get; set; }        

        public Movie.List Movies { get; set; }
        public PersonsMovieCredits.IntDictionary PersonsMovieCreditsById { get; set; }        
        public MoviesCredits.IntDictionary MoviesCreditsById { get; set; }

        [JsonIgnore]
        public int SaveFrequencyPerS { get; set; }
        [JsonIgnore]
        public int SavePeriodMs => MsPerS / SaveFrequencyPerS;

        [JsonIgnore]
        public bool WriteBackupFile { get; set; }
        [JsonIgnore]
        public bool ClearUnneededData { get; set; }

        [JsonIgnore]
        public int MoviesFetched { get; set; }
        [JsonIgnore]
        public int MovieCreditsFetched { get; set; }
        [JsonIgnore]
        public int PersonMoviesCreditsFetched { get; set; }

        public int MovieCount => Movies.Count;
        public int MovieCreditsCount => MoviesCreditsById.Count;
        public int PersonMoviesCreditsCount => PersonsMovieCreditsById.Count;        
        
        private const int MsPerS = 1000;               

        private readonly object _fileLockObj = new();

        private readonly Timer _saveTimer;

        private bool _disposed;
        private bool _stopped;
        private bool _started;

        public JsonFileCache()
        {
            Movies = new ();
            PersonsMovieCreditsById = new ();
            MoviesCreditsById = new ();

            SaveFrequencyPerS = 4;
            WriteBackupFile = true;
            ClearUnneededData = true;

            _started = false;
            _stopped = false;
            _disposed = false;

            // no state is passed, and timer is started for one cycle/non-periodic (we restart the timer one cycle at a time)
            _saveTimer = new Timer(new TimerCallback(TimerCallback), null, Timeout.Infinite, Timeout.Infinite);

            //Start();
        }    
             
        public JsonFileCache(string filePath)
            : this()
        {
            FilePath = filePath;
        }

        public async Task AddCreditsForMovieAsync(MoviesCredits moviesCredits)
        {
            await Task.Run(() =>
            {
                AddCreditsForMovie(moviesCredits);
            });
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await Task.Run(() =>
            {
                AddMovie(movie);
            });
        }

        public async Task AddPersonsMovieCreditsAsync(PersonsMovieCredits personsMovieCredits)
        {
            await Task.Run(() =>
            {
                AddPersonsMovieCredits(personsMovieCredits);
            });
        }

        public void AddCreditsForMovie(MoviesCredits moviesCredits)
        {
            if (!MoviesCreditsById.ContainsKey(moviesCredits.MovieId))
            {
                MoviesCreditsById[moviesCredits.MovieId] = moviesCredits;
                Save();
            }            
        }

        public void AddMovie(Movie movie)
        {
            if (!Movies.Contains(movie))
            {
                Movies.Add(movie);
                Save();
            }            
        }

        public void AddPersonsMovieCredits(PersonsMovieCredits personsMovieCredits)
        {
            if (!PersonsMovieCreditsById.ContainsKey(personsMovieCredits.PersonId))
            {
                PersonsMovieCreditsById[personsMovieCredits.PersonId] = personsMovieCredits;
                Save();
            }
        }

        public async Task<MoviesCredits> GetCreditsForMovieAsync(int movieId)
        {
            MoviesCredits instance = null;
            await Task.Run(() =>
            {
                if (MoviesCreditsById.ContainsKey(movieId))
                {
                    instance = MoviesCreditsById[movieId];
                    MovieCreditsFetched++;
                }
            });
            return instance;
        }

        public async Task<Movie> GetMovieAsync(string title, int releaseYear)
        {
            Movie instance = null;
            await Task.Run(() =>
            {
                instance = Movies.Find(m =>
                {
                    return m.Title == title &&
                           m.ReleaseYear == releaseYear;
                });
                MoviesFetched++;                
            });
            return instance;
        }

        public Movie GetMovie(int movieId)
        {
            var movie = Movies.Find(m =>
            {
                return m.ApiId == movieId;
            });
            MoviesFetched++;
            return movie;
        }

        public async Task<PersonsMovieCredits> GetMovieCreditsForPersonAsync(int personId)
        {
            PersonsMovieCredits instance = null;
            await Task.Run(() =>
            {
                if (PersonsMovieCreditsById.ContainsKey(personId))
                {
                    instance = PersonsMovieCreditsById[personId];
                    PersonMoviesCreditsFetched++;
                }
            });
            return instance;
        }

        public static JsonFileCache Load(string filePath)
        {
            //try
            {
                var fileContent = File.ReadAllText(filePath);                                                
                return JsonSerializer.Deserialize<JsonFileCache>(fileContent, GlobalSerializerOptions.Options);
                //instance = JsonSerializer.Deserialize(fileContent, typeof(JsonFileCache), new JsonFileCacheSerializationContext(GlobalSerializerOptions.Options)) as JsonFileCache;
            }
            //catch (FileNotFoundException)
            {
            //    return null;
            }            
        }

        public void Clear()
        {
            Movies.Clear();
            PersonsMovieCreditsById.Clear();
            MoviesCreditsById.Clear();
        }

        private Task SerializeToFile()
        {            
            // filePath not used, get path from the member property
            lock (_fileLockObj)
            {
                // TODO: use JsonSerializerContext-dervied class to optimize serialization
                //var json = JsonSerializer.Serialize(this, typeof(JsonFileCache), new JsonFileCacheSerializationContext(GlobalSerializerOptions.Options));

                if (ClearUnneededData)
                {
                    ClearUneededData();
                }

                var json = JsonSerializer.Serialize(this, GlobalSerializerOptions.Options);
                File.WriteAllText(FilePath, json);
                if (WriteBackupFile)
                {
                    // write a backup so if one gets corrupted by an error during write, the other should still be OK
                    File.WriteAllText(FilePath.Replace(".json", string.Empty) + ".bak.json", json);
                }
            }      
            return Task.CompletedTask;
        }

        /// <summary>
        /// Clear out (set to null) all unneeded data fields to reduce file size
        /// </summary>
        private void ClearUneededData()
        {
            // set empty string b/c its 2 characters shorter than "null"
            const string valueToSet = default;

            foreach (var movieCredit in MoviesCreditsById.Values)
            {                
                foreach (var cast in movieCredit.Credits.Cast)
                {
                    // DO NOT clear the Id field!
                    //cast.Id = default;
                    cast.CreditId = valueToSet;
                    cast.KnownForDepartment = valueToSet;
                    cast.OriginalName = valueToSet;                    
                }
                foreach (var crew in movieCredit.Credits.Crew)
                {
                    //crew.Id = default;
                    crew.CreditId = valueToSet;
                    crew.KnownForDepartment = valueToSet;
                    crew.OriginalName = valueToSet;
                }
            }
            foreach (var personMovieCredit in PersonsMovieCreditsById.Values)
            {
                foreach (var cast in personMovieCredit.MovieCredits.Cast)
                {
                    //cast.Id = default;
                    cast.CreditId = valueToSet;
                    cast.OriginalTitle = valueToSet;
                }
                foreach (var crew in personMovieCredit.MovieCredits.Crew)
                {
                    //crew.Id = default;
                    crew.CreditId = valueToSet;
                    crew.OriginalTitle = valueToSet;
                }
            }
        }

        public void Save()
        {
            // do nothing- saving is handled in the timer callback!
            //SerializeToFile(FilePath);
        }

        public async Task SaveAsync()
        {
            using var ms = new MemoryStream();
            //await JsonSerializer.SerializeAsync(ms, this, typeof(JsonFileCache), new JsonFileCacheSerializationContext(GlobalSerializerOptions.Options)); 
            await JsonSerializer.SerializeAsync(ms, this, GlobalSerializerOptions.Options);

            lock (_fileLockObj)
            {
                using var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.Read);
                ms.Position = 0;
                fs.CopyTo(ms);
                fs.Flush();
                fs.Close();
            }
        }

        private void TimerCallback(object state)
        {
            if (!_stopped)
            {
                SerializeToFile();
                // kick the timer off again for one cycle
                if (!_disposed)
                {
                    _saveTimer.Change(SavePeriodMs, 0);
                }
            }
        }

        public void Start()
        {
            if (!_started)
            {
                _stopped = false;
                //start the timer for one cycle
                _saveTimer.Change(SavePeriodMs, Timeout.Infinite);
                _started = true;
            }
        }

        public void Stop()
        {
            _stopped = true;
            // stop the timer
            _saveTimer.Change(0, 0);            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Stop();
                    _saveTimer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposed = true;
            }
        }     

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
