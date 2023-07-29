using System;
using System.Collections.Generic;

namespace MovieMatchMakerLib.Model
{
    public class Role : IEquatable<Role>
    {
        public Movie Movie { get; set; }
        public string Job { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Role);
        }

        public bool Equals(Role other)
        {
            return other is not null &&
                   EqualityComparer<Movie>.Default.Equals(Movie, other.Movie) &&
                   Job == other.Job;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Movie, Job);
        }

        public static bool operator ==(Role left, Role right)
        {
            return EqualityComparer<Role>.Default.Equals(left, right);
        }

        public static bool operator !=(Role left, Role right)
        {
            return !(left == right);
        }

        public class List : List<Role>
        {
            public List()
                : base() 
            { }
            public List(IEnumerable<Role> roles)
                : base(roles)
            { }
        }

        public class HashSet : HashSet<Role>
        {
            public HashSet()
                : base()
            { }
            public HashSet(IEnumerable<Role> roles)
                : base(roles)
            { }
        }
    }
}
