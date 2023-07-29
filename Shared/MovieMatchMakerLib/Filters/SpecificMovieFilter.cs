using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Filters
{
    public class SpecificMovieFilter : MovieConnectionListFilterBase
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }

        public SpecificMovieFilter(string title, int releaseYear)
        {
            Title = title;
            ReleaseYear = releaseYear;
        }   

        protected override MovieConnection.List FilterList(MovieConnection.List input)
        {
           return input.FindForMovie(Title, ReleaseYear);
        }
    }
}
