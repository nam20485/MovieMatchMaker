using System.Threading.Tasks;

namespace MovieMatchMakerLib
{
    public interface IMovieNetworkDataBuilder
    {
        Task BuildFromInitial(string title, int releaseYear, int degree);
    }
}