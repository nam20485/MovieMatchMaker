using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace MovieMatchMakerLib.Model
{
    public class Name : IEquatable<Name>
    {
        //[Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }        

        public string FullName
        {
            get
            {
                var sb = new StringBuilder(FirstName);
                if (!string.IsNullOrEmpty(Surname))
                {
                    sb.Append($" {Surname}");
                }
                return sb.ToString();
            }
        }

        public Name()
        {
            // required by JSON deserialization
        }

        public Name(string firstName, string surname)
        {
            FirstName = firstName;
            Surname = surname;
        }

        public Name(string fullName)
            : this("", "")
        {
            ParseFullName(fullName);
        }

        private void ParseFullName(string fullName)
        {
            if (fullName.Contains(","))
            {
                var parts = fullName.Split(',');
                if (parts.Length > 0)
                {
                    Surname = parts[0];
                }
                if (parts.Length > 1)
                {
                    FirstName = parts[1].Trim();
                }
            }
            else
            {
                var parts = fullName.Split(' ');
                if (parts.Length > 0)
                {
                    FirstName = parts[0];
                }
                if (parts.Length > 1)
                {
                    Surname = parts[1];
                }
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Name);
        }

        public bool Equals(Name other)
        {
            return !(other is null) &&
                   FirstName == other.FirstName &&
                   Surname == other.Surname;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, Surname);
        }

        public static bool operator ==(Name left, Name right)
        {
            return EqualityComparer<Name>.Default.Equals(left, right);
        }

        public static bool operator !=(Name left, Name right)
        {
            return !(left == right);
        }
    }
}
