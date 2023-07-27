using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib.Model
{
    public class MovieConnection : IEquatable<MovieConnection>
    {
        public Movie SourceMovie { get; set; }
        public Movie TargetMovie { get; set; }

        public ConnectedRole.List ConnectedRoles { get; set; }

        public int Id { get; set; }

        public MovieConnection()
        { 
            // required for deserialization
        }

        public MovieConnection(Movie sourceMovie, Movie targetMovie)
        {
            SourceMovie = sourceMovie;
            TargetMovie = targetMovie;
            ConnectedRoles = new ConnectedRole.List();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MovieConnection);
        }

        public bool Equals(MovieConnection other)
        {
            return !(other is null) &&
                   // allow for reversed source and target movies (they are the "same" connection)
                   ((EqualityComparer<Movie>.Default.Equals(SourceMovie, other.SourceMovie) &&
                     EqualityComparer<Movie>.Default.Equals(TargetMovie, other.TargetMovie)) ||
                    (EqualityComparer<Movie>.Default.Equals(SourceMovie, other.TargetMovie) &&
                     EqualityComparer<Movie>.Default.Equals(TargetMovie, other.SourceMovie)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SourceMovie, TargetMovie);
        }

        public static bool operator ==(MovieConnection left, MovieConnection right)
        {
            return EqualityComparer<MovieConnection>.Default.Equals(left, right);
        }

        public static bool operator !=(MovieConnection left, MovieConnection right)
        {
            return !(left == right);
        }

        public class List : List<MovieConnection>
        {
            public static readonly List Empty = new();

            public List()
                : base()
            {
            }

            public List(IEnumerable<MovieConnection> collection)
                : base(collection)
            {
            }

            public Movie.HashSet Movies
            {
                get
                {
                    var movies = new Movie.HashSet();
                    foreach (var connection in this)
                    {
                        if (!movies.Contains(connection.SourceMovie))
                        {
                            movies.Add(connection.SourceMovie);
                        }
                        if (!movies.Contains(connection.TargetMovie))
                        {
                            movies.Add(connection.TargetMovie);
                        }
                    }
                    return movies;
                }
            }

            public List FindForMovie(string title, int releaseYear)
            {
                var genericList = FindAll(mc =>
                {
                    return (mc.SourceMovie.Title == title && mc.SourceMovie.ReleaseYear == releaseYear) ||
                           (mc.TargetMovie.Title == title && mc.TargetMovie.ReleaseYear == releaseYear);
                });

                var mcList = new List(genericList);
                return mcList;
            }

            public MovieConnection FindConnection(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle, int targetMovieReleaseYear)
            {
                return Find(mc =>
                {
                    return ((mc.SourceMovie.Title == sourceMovieTitle &&
                             mc.SourceMovie.ReleaseYear == sourceMovieReleaseYear &&
                             mc.TargetMovie.Title == targetMovieTitle &&
                             mc.TargetMovie.ReleaseYear == targetMovieReleaseYear) ||
                            (mc.SourceMovie.Title == targetMovieTitle &&
                             mc.SourceMovie.ReleaseYear == targetMovieReleaseYear &&
                             mc.TargetMovie.Title == sourceMovieTitle &&
                             mc.TargetMovie.ReleaseYear == sourceMovieReleaseYear));
                });
            }

            //public MovieConnection FindConnectionExact(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle, int targetMovieReleaseYear)
            //{
            //    return Find(mc =>
            //    {
            //        return (mc.SourceMovie.Title == sourceMovieTitle &&
            //                 mc.SourceMovie.ReleaseYear == sourceMovieReleaseYear &&
            //                 mc.TargetMovie.Title == targetMovieTitle &&
            //                 mc.TargetMovie.ReleaseYear == targetMovieReleaseYear);
            //    });
            //}

            private MovieConnection GetAt(int index)
            {
                if (index < Count)
                {
                    return this[index];
                }
                return null;
            }

            public MovieConnection FindConnection(int id)
            {
                // TODO: use find by id once MovieConnection's have their Id set
                return GetAt(id);
                //return Find(mc => mc.Id == id);
            }

            public List Filter(IMovieConnectionListFilter filter)
            {
                return filter.Apply(this);
            }

            public List Filter(IEnumerable<IMovieConnectionListFilter> filters)
            {
                var filtered = this;
                foreach (var filter in filters)
                {
                    filtered = filtered.Filter(filter);
                }
                return filtered;
            }

            public string ToJson()
            {
                return JsonSerializer.Serialize(this, MyJsonSerializerOptions.JsonSerializerOptions);
            }

            public static List FromJson(string json)
            {
                return JsonSerializer.Deserialize<List>(json, MyJsonSerializerOptions.JsonSerializerOptions);
            }

            public void SaveToFile(string path)
            {
                File.WriteAllText(path, ToJson());
            }

            public static List LoadFromFile(string path)
            {
                return FromJson(File.ReadAllText(path));
            }

            public MovieConnection GetOrCreateMovieConnection(Movie sourceMovie, Movie targetMovie)
            {
                var movieConnection = FindConnection(sourceMovie.Title, sourceMovie.ReleaseYear, targetMovie.Title, targetMovie.ReleaseYear);
                if (movieConnection is null)
                {
                    // not found, return an empty new one
                    movieConnection = new MovieConnection(sourceMovie, targetMovie)
                    {
                        // set a unique id
                        Id = Count
                    };
                    Add(movieConnection);
                }

                return movieConnection;
            }

            public bool Contains(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle, int targetMovieReleaseYear)
            {
                return FindConnection(sourceMovieTitle, sourceMovieReleaseYear, targetMovieTitle, targetMovieReleaseYear) != null;
            }               
        }
    }
}
