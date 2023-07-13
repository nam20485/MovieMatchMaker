using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieMatchMakerLib
{
    public class MovieConnection
    {
        public Movie SourceMovie { get; set; }
        public Movie TargetMovie { get; set; }  
        
        public ConnectedRole.List ConnectedRoles {  get; set; }

        public MovieConnection(Movie sourceMovie, Movie targetMovie)
        {
            SourceMovie = sourceMovie;
            TargetMovie = targetMovie;
            ConnectedRoles = new ConnectedRole.List();
        }

        public class StringDictionary : Dictionary<string, MovieConnection>
        {
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

            public void SortDescending()
            {
                Sort((mc1, mc2) =>
                {
                    return mc2.ConnectedRoles.Count.CompareTo(mc1.ConnectedRoles.Count);
                });
            }

            public List GetMeaningfulConnections(int degree)
            {
                var genericList = FindAll(mc =>
                {
                    return mc.ConnectedRoles.Count >= degree;
                });
                return new List(genericList);
            }

            public MovieConnection.List GetNonCloselyRelatedConnections()
            {
                var genericList = FindAll(mc =>
                {
                    return StringsArentSimilar(mc.SourceMovie.Title, mc.TargetMovie.Title);
                });
                return new List(genericList);
            }

            private static readonly string[] commonWords =
            {
                "and", "but", "or",
                "a", "an",
                "of",
                "the",
                "if",
            };

            private static bool StringsArentSimilar(string str1, string str2)
            {
                var words1 = str1.Split(' ');
                foreach (var word1 in words1)
                {
                    if (!commonWords.Contains(word1))
                    {
                        if (str2.Contains(word1))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
