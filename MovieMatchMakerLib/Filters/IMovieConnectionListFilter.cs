using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Filters
{
    public interface IMovieConnectionListFilter
    {
        MovieConnection.List Apply(MovieConnection.List input);        

    }
}
