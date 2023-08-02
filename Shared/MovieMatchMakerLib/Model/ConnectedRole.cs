using System;
using System.Collections.Generic;
using MovieMatchMakerLib.TmdbApi;

namespace MovieMatchMakerLib.Model
{
    public class ConnectedRole : IEquatable<ConnectedRole>, ITmdbLinkable
    {
        public Name Name { get; set; }
        public int PersonId { get; set; }
        public string TargetJob { get; set; }
        public string SourceJob { get; set; }
        public string PersonPosterPathSuffix { get; set; }
        public string PersonPosterPath => TmdbApiHelper.MakeImagePath(TmdbApiHelper.PosterImageSize.w92, PersonPosterPathSuffix);

        public string TmdbLink => TmdbApiHelper.MakeTmdbUrl("person", PersonId);

        public override bool Equals(object obj)
        {
            return Equals(obj as ConnectedRole);
        }

        public bool Equals(ConnectedRole other)
        {
            return !(other is null) &&
                   EqualityComparer<Name>.Default.Equals(Name, other.Name) &&
                   PersonId == other.PersonId &&
                   // allow for reversed jobs
                   (TargetJob == other.TargetJob && SourceJob == other.SourceJob ||
                    TargetJob == other.SourceJob && SourceJob == other.TargetJob);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, PersonId, TargetJob, SourceJob);
        }

        public static bool operator ==(ConnectedRole left, ConnectedRole right)
        {
            return EqualityComparer<ConnectedRole>.Default.Equals(left, right);
        }

        public static bool operator !=(ConnectedRole left, ConnectedRole right)
        {
            return !(left == right);
        }

        public class NameDictionary : Dictionary<Name, ConnectedRole>
        {
        }

        public class List : List<ConnectedRole>
        {
        }
    }
}
