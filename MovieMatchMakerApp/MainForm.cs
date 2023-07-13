using Microsoft.EntityFrameworkCore.Infrastructure;

using MovieMatchMakerLib;
using MovieMatchMakerLib.Filters;

namespace MovieMatchMakerApp
{
    public partial class MainForm : Form
    {
        private const string Title = "Movie Match Maker";
        private const string Slogan = "Find movies you didn't know you liked";       

        private static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string FilePath => Path.Combine(AppDataPath, "moviematchmaker.json");

        private readonly MovieNetworkDataBuilder _movieNetworkDataBuilder;
        private readonly IDataCache _dataCache;
        private MovieConnectionBuilder _connectionBuilder;

        public MainForm()
        {
            _dataCache = JsonFileCache.Load(FilePath);
            var apiDataSource = new ApiDataSource();
            var cachedDataSource = new CachedDataSource(_dataCache, apiDataSource);
            _movieNetworkDataBuilder = new MovieNetworkDataBuilder(cachedDataSource);
            _connectionBuilder = new MovieConnectionBuilder(_dataCache);

            InitializeComponent();

            Text = $"{Title} - {Slogan}!";
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UseWaitCursor = true;

            //await _connectionManager.FindMoviesConnectedToMovie("Dark City", 1998, 1);
            //await _dataCache.SaveAsync();
            //
            await _connectionBuilder.FindMovieConnections();

            var sorted = new SortFilter().Apply(_connectionBuilder.MovieConnections);
            var greaterThan2ConnectedRoles = new MinConnectedRolesCountFilter().Apply(_connectionBuilder.MovieConnections);
            var greaterThan4ConnectedRoles = new MinConnectedRolesCountFilter(4).Apply(_connectionBuilder.MovieConnections);
            var max0MatchingTitleWords = new MaxMatchingTitleWordsFilter(0).Apply(_connectionBuilder.MovieConnections);
            var max1MatchingTitleWords = new MaxMatchingTitleWordsFilter(1).Apply(_connectionBuilder.MovieConnections);

            var allFilters = 
                new MaxMatchingTitleWordsFilter().Apply(
                    new MinConnectedRolesCountFilter().Apply(
                        new SortFilter().Apply(_connectionBuilder.MovieConnections)));
                

            //var nonCloselyRelatedConnections = _connectionBuilder.MovieConnections.GetNonCloselyRelatedConnections();

            UseWaitCursor = false;

            //Close();
        }
    }
}