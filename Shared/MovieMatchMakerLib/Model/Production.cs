﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MovieMatchMakerLib.TmdbApi;

namespace MovieMatchMakerLib.Model
{
    public abstract class Production : IEquatable<Production>, ITmdbLinkable
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int ApiId { get; set; }
        public string PosterImagePathSuffix { get; set; }

        [JsonIgnore]
        public bool Fetched { get; set; }        
        [JsonIgnore]
        public string DisplayId => string.Format(DisplayIdFormat, Title, ReleaseYear);
        [JsonIgnore]
        public string PosterImagePath => TmdbApiHelper.MakeImagePath(TmdbApiHelper.PosterImageSize.w92, PosterImagePathSuffix);

        [JsonIgnore]
        public abstract string TmdbLink { get; }

        private const string DisplayIdFormat = "{0} ({1})";

        public Production()
        {
            // required for deserialization
        }

        public Production(string title, int releaseYear, int movieId, string posterImagePath)
        {
            Title = title;
            ReleaseYear = releaseYear;
            ApiId = movieId;
            PosterImagePathSuffix = posterImagePath;
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

        public string MakePosterImagePath(TmdbApiHelper.PosterImageSize posterImageSize)
        {
            return TmdbApiHelper.MakeImagePath(posterImageSize, PosterImagePathSuffix);
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
