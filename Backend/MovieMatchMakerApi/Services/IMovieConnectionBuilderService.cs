using MovieMatchMakerLib;

namespace MovieMatchMakerApi.Services
{
    public interface IMovieConnectionBuilderService
    {
        MovieConnectionBuilder MovieConnectionBuilder { get; }
    }
}
