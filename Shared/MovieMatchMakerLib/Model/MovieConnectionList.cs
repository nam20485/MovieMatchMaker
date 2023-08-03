using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using MovieMatchMakerLib.Filters;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib.Model
{
    public partial class MovieConnection
    {
        public partial class List : List<MovieConnection>
        {
            public static readonly List Empty = new();

            private readonly object _accessLock = new();

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
                        movies.Add(connection.SourceMovie);
                        movies.Add(connection.TargetMovie);                        
                    }
                    return movies;
                }
            }

            public Role.HashSet GetRolesForPerson(Name name)
            {
                var roles = new Role.HashSet();
                foreach (var movieConnection in this)
                {
                    foreach (var connectedRole in movieConnection.ConnectedRoles)
                    {
                        if (connectedRole.Name == name)
                        {
                            roles.Add(new Role()
                            {
                                Movie = movieConnection.SourceMovie,
                                Job = connectedRole.SourceJob
                            });
                            roles.Add(new Role()
                            {
                                Movie = movieConnection.TargetMovie,
                                Job = connectedRole.TargetJob
                            });                            
                        }
                    }
                }
                return roles;
            }

            public Role.HashSet GetRolesForPerson(int personId)
            {
                if (FindPersonName(personId) is Name name)
                {
                    return GetRolesForPerson(name);
                }
                return null;
            }

            public int FindPersonId(Name name)
            {
                foreach (var movieConnection in this)
                {
                    foreach (var connectedRole in movieConnection.ConnectedRoles)
                    {
                        if (connectedRole.Name == name)
                        {
                            return connectedRole.PersonId;
                        }
                    }
                }
                return -1;
            }

            public Name FindPersonName(int personId)
            {
                foreach (var movieConnection in this)
                {
                    foreach (var connectedRole in movieConnection.ConnectedRoles)
                    {
                        if (connectedRole.PersonId == personId)
                        {
                            return connectedRole.Name;
                        }
                    }
                }
                return null;
            }

            public List FindForMovie(string title, int releaseYear)
            {
                return new List(FindAll(mc =>
                {
                    return (mc.SourceMovie.Title == title && mc.SourceMovie.ReleaseYear == releaseYear) ||
                           (mc.TargetMovie.Title == title && mc.TargetMovie.ReleaseYear == releaseYear);
                }));
            }

            public MovieConnection FindConnection(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle, int targetMovieReleaseYear)
            {
                lock (_accessLock)
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
            }

            public MovieConnection FindConnectionExact(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle, int targetMovieReleaseYear)
            {
                return Find(mc =>
                {
                    return (mc.SourceMovie.Title == sourceMovieTitle &&
                             mc.SourceMovie.ReleaseYear == sourceMovieReleaseYear &&
                             mc.TargetMovie.Title == targetMovieTitle &&
                             mc.TargetMovie.ReleaseYear == targetMovieReleaseYear);
                });
            }

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
                return JsonSerializer.Serialize(this, GlobalSerializerOptions.Options);
            }

            public static List FromJson(string json)
            {
                return JsonSerializer.Deserialize<List>(json, GlobalSerializerOptions.Options);
            }

            public void SaveToFile(string path)
            {
                File.WriteAllText(path, ToJson());
            }

            public static List LoadFromFile(string path)
            {
                try
                {
                    return FromJson(File.ReadAllText(path));
                }
                catch (Exception e)
                {
                    ErrorLog.Log(e);
                    return null;
                }
            }

            public MovieConnection GetOrCreateMovieConnection(Movie sourceMovie, Movie targetMovie)
            {
                lock (_accessLock)
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
            }

            public bool Contains(string sourceMovieTitle, int sourceMovieReleaseYear, string targetMovieTitle, int targetMovieReleaseYear)
            {
                return FindConnection(sourceMovieTitle, sourceMovieReleaseYear, targetMovieTitle, targetMovieReleaseYear) != null;
            }
        }
    }
}
