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
        public string PosterImagePath { get; set; }


        private const string DisplayIdFormat = "{0} ({1})";
        public string DisplayId => string.Format(DisplayIdFormat, Title, ReleaseYear);

        public Production(string title, int releaseYear, int movieId, string posterImagePath)
        {
            Title = title;
            ReleaseYear = releaseYear;
            MovieId = movieId;
            PosterImagePath = posterImagePath;
            Fetched = false;
        }

        public override string ToString() => DisplayId;

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
