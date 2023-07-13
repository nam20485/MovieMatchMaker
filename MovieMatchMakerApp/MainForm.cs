using Microsoft.EntityFrameworkCore.Infrastructure;

using MovieMatchMakerLib;

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
            _connectionBuilder.MovieConnections.SortDescending();

            var meaningfulConnections = _connectionBuilder.MovieConnections.GetMeaningfulConnections(2);
            var nonCloselyRelatedConnections = _connectionBuilder.MovieConnections.GetNonCloselyRelatedConnections();

            UseWaitCursor = false;

            //Close();
        }
    }
}