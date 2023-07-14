using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieMatchMakerLib.Filters
{
    public class MaxMatchingTitleWordsFilter : MovieConnectionListFilter
    {
        public int MaxMatchingTitleWords { get; private set; }

        public MaxMatchingTitleWordsFilter() 
            : this(0)
        {
        }

        public MaxMatchingTitleWordsFilter(int maxMatchingTitleWords)
        {
            MaxMatchingTitleWords = maxMatchingTitleWords;
        }

        protected override MovieConnection.List FilterList(MovieConnection.List list)
        {
            var genericList = list.FindAll(mc =>
            {                
                return CountMatchingWords(mc.SourceMovie.Title, mc.TargetMovie.Title) <= MaxMatchingTitleWords;
            });
            return new MovieConnection.List(genericList);
        }

        private static readonly string[] commonWords =
        {
                "and", "but", "or",
                "a", "an",
                "of",
                "the",
                "if",
        };

        private static int CountMatchingWords(string str1, string str2)
        {
            int numMatchingWords = 0;

            var words1 = str1.Split(' ');
            foreach (var word1 in words1)
            {
                if (!commonWords.Contains(word1))
                {
                    if (str2.Contains(word1))
                    {
                        numMatchingWords++;
                    }
                }
            }

            return numMatchingWords;
        }
    }
}
