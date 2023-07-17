using System;
using System.Collections.Generic;

namespace MovieMatchMakerLib.Model
{
    public class Production : IEquatable<Production>
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int MovieId { get; set; }
        public bool Fetched { get; set; }

        public Production(string title, int releaseYear, int movieId)
        {
            Title = title;
            ReleaseYear = releaseYear;
            MovieId = movieId;
            Fetched = false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Production);
        }

        public bool Equals(Production other)
        {
            return !(other is null) &&
                   Title == other.Title &&
                   ReleaseYear == other.ReleaseYear;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, ReleaseYear);
        }

        public static bool operator ==(Production left, Production right)
        {
            return EqualityComparer<Production>.Default.Equals(left, right);
        }

        public static bool operator !=(Production left, Production right)
        {
            return !(left == right);
        }
    }
}
