using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatchMakerLib.Filters
{
    public interface IMovieConnectionListFilter
    {
        MovieConnection.List Apply(MovieConnection.List input);        

    }
}
