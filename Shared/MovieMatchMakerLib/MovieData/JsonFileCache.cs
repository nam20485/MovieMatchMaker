using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using MovieMatchMakerLib.Model;
using MovieMatchMakerLib.Utils;


namespace MovieMatchMakerLib.Data
{
    public class JsonFileCache : IDataCache
    {
        public string FilePath { get; set; }

        public Movie.List Movies { get; set; }
        public PersonsMovieCredits.IntDictionary PersonsMovieCreditsById { get; set; }        
        public MoviesCredits.IntDictionary MoviesCreditsById { get; set; }

        [JsonIgnore]
        public int MoviesFetched { get; set; }
        [JsonIgnore]
        public int MovieCreditsFetched { get; set; }
        [JsonIgnore]
        public int PersonMoviesCreditsFetched { get; set; }

        public int MovieCount => Movies.Count;
        public int MovieCreditsCount => MoviesCreditsById.Count;
        public int PersonMoviesCreditsCount => PersonsMovieCreditsById.Count;

        private readonly object _fileLockObj = new();

        private RequestProcessingLoopThread<string> _serializeToFileLoopThread;

        public JsonFileCache()
        {
            Movies = new ();
            PersonsMovieCreditsById = new ();
            MoviesCreditsById = new ();

            //_serializeToFileLoopThread = new RequestProcessingLoopThread<string>(SerializeToFile, false);
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

            //await SaveAsync();
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await Task.Run(() =>
            {
                AddMovie(movie);
            });

            //await SaveAsync();
        }

        public async Task AddPersonsMovieCreditsAsync(PersonsMovieCredits personsMovieCredits)
        {
            await Task.Run(() =>
            {
                AddPersonsMovieCredits(personsMovieCredits);
            });

            //await SaveAsync();
        }

        public void AddCreditsForMovie(MoviesCredits moviesCredits)
        {
            if (!MoviesCreditsById.ContainsKey(moviesCredits.MovieId))
            {
                MoviesCreditsById[moviesCredits.MovieId] = moviesCredits;
                Save();
                //await SaveAsync();
            }            
        }

        public void AddMovie(Movie movie)
        {
            if (!Movies.Contains(movie))
            {
                Movies.Add(movie);
                Save();
                //await SaveAsync();
            }            
        }

        public void AddPersonsMovieCredits(PersonsMovieCredits personsMovieCredits)
        {
            if (!PersonsMovieCreditsById.ContainsKey(personsMovieCredits.PersonId))
            {
                PersonsMovieCreditsById[personsMovieCredits.PersonId] = personsMovieCredits;
                Save();
                //await SaveAsync();
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
                return m.MovieId == movieId;
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
            var instance = new JsonFileCache();
            try
            {
                var fileContent = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(fileContent))
                {
                    //instance = JsonSerializer.Deserialize(fileContent, typeof(JsonFileCache), new JsonFileCacheSerializationContext(GlobalSerializerOptions.Options)) as JsonFileCache;
                    instance = JsonSerializer.Deserialize<JsonFileCache>(fileContent, GlobalSerializerOptions.Options);
                }
            }
            catch (FileNotFoundException)
            {
                // do nothing (no previous cache exists)
            }
            instance.FilePath = filePath;
            return instance;
        }

        public void Clear()
        {
            Movies.Clear();
            PersonsMovieCreditsById.Clear();
            MoviesCreditsById.Clear();
        }

        private Task SerializeToFile(string filePath)
        {
            lock (_fileLockObj)
            {
                // TODO: use JsonSerializerContext-dervied class to optimize serialization
                //var json = JsonSerializer.Serialize(this, typeof(JsonFileCache), new JsonFileCacheSerializationContext(GlobalSerializerOptions.Options));
                var json = JsonSerializer.Serialize(this);
                File.WriteAllText(FilePath, json);
                // write a backup so if one gets corrupted by an error during write, the other should still be OK
                File.WriteAllText(FilePath + ".bak.json", json);
            }
            return Task.CompletedTask;
        }

        public void Save()
        {
            //_serializeToFileLoopThread.AddRequest(FilePath);
            SerializeToFile(FilePath);
        }

        public async Task SaveAsync()
        {
            using (var ms = new MemoryStream())
            {
                //await JsonSerializer.SerializeAsync(ms, this, typeof(JsonFileCache), new JsonFileCacheSerializationContext(GlobalSerializerOptions.Options)); 
                await JsonSerializer.SerializeAsync(ms, this, GlobalSerializerOptions.Options);

                lock (_fileLockObj)
                {
                    using (var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        ms.Position = 0;
                        fs.CopyTo(ms);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }
        }
    }
}
