using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatchMakerLib.Filters
{
    public abstract class MovieConnectionListFilter : IMovieConnectionListFilter
    {
        public MovieConnection.List Apply(MovieConnection.List input)
        {
            var listCopy = new MovieConnection.List(input);
            var filteredList = FilterList(listCopy);
            return filteredList;

            //return FilterList(new MovieConnection.List(input));
        }

        protected abstract MovieConnection.List FilterList(MovieConnection.List list);
    }
}
