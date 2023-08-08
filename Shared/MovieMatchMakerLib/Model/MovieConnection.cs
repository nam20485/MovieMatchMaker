using System;
using System.Collections.Generic;

namespace MovieMatchMakerLib.Model
{
    public partial class MovieConnection : IEquatable<MovieConnection>
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

        public partial class List : List<MovieConnection>
        {
            // MovieConnectionList.cs            
        }
    }
}
