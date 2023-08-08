namespace MovieMatchMakerLib.Filters
{
    public class DefaultMovieConnectionListFilters
    {
        public static IMovieConnectionListFilter[] Filters => _defaultFilters;

        private static readonly IMovieConnectionListFilter[] _defaultFilters = new IMovieConnectionListFilter[]
        {
            new MinConnectedRolesCountFilter(5),
            //new MaxMatchingTitleWordsFilter(3)
        };
    }
}
