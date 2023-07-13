using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib
{
    public class JsonFileCache : IDataCache
    {
        public string FilePath { get; set; }       
        public Movie.List Movies { get; set; }
        public PersonsMovieCredits.IntDictionary PersonsMovieCreditsById { get; set; }
        public MoviesCredits.IntDictionary MoviesCreditsById { get; set; }

        private object _lockObj = new object();

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            //ReferenceHandler = ReferenceHandler.Preserve,
        };

        public JsonFileCache()
        {
            Movies = new Movie.List();
            PersonsMovieCreditsById = new PersonsMovieCredits.IntDictionary();
            MoviesCreditsById = new MoviesCredits.IntDictionary();
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
            if (! PersonsMovieCreditsById.ContainsKey(personsMovieCredits.PersonId))
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
                    return (m.Title == title &&
                            m.ReleaseYear == releaseYear);
                });
            });
            return instance;
        }

        public Movie GetMovie(int movieId)
        {
            var movie = Movies.Find(m =>
            {
                return m.MovieId == movieId;
            });
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
                    instance = JsonSerializer.Deserialize<JsonFileCache>(fileContent);
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

        public void Save()
        {            
            lock (_lockObj)
            {
                var json = JsonSerializer.Serialize(this, _jsonSerializerOptions);
                File.WriteAllText(FilePath, json);               
            }
        }

        public async Task SaveAsync()
        {
            using (var ms = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(ms, this, _jsonSerializerOptions);

                lock (_lockObj)
                {
                    using (var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                    {
                        ms.Position = 0;
                        fs.CopyTo(ms);
                        //fs.Flush();
                        fs.Close();
                    }
                }
            }        
        }             
    }
}
