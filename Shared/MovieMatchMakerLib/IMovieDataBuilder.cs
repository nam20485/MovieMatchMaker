using System.Threading.Tasks;

namespace MovieMatchMakerLib
{
    public interface IMovieDataBuilder
    {
        Task BuildFromInitial(string title, int releaseYear, int degree);
    }
}