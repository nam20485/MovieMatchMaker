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
                   EqualityComparer<Movie>.Default.Equals(SourceMovie, other.SourceMovie) &&
                    EqualityComparer<Movie>.Default.Equals(TargetMovie, other.TargetMovie) ||
                   EqualityComparer<Movie>.Default.Equals(SourceMovie, other.TargetMovie) &&
                    EqualityComparer<Movie>.Default.Equals(TargetMovie, other.SourceMovie);
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
            public List()
                : base()
            {
            }

            public List(IEnumerable<MovieConnection> collection)
                : base(collection)
            {
            }

            public List Filter(IMovieConnectionListFilter filter)
            {
                return filter.Apply(this);
            }

            public List Filter(List<IMovieConnectionListFilter> filters)
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
        }
    }
}
