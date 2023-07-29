using System.Threading.Tasks;

namespace MovieMatchMakerLib
{
    public interface IMovieDataBuilder
    {
        void BuildFromInitial(string title, int releaseYear, int degree);
    }
}