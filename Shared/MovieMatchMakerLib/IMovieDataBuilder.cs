namespace MovieMatchMakerLib
{
    public interface IMovieDataBuilder
    {
        void BuildFreshFromInitial(string title, int releaseYear, int degree);
        void ContinueFromExisting(int degree);
    }
}