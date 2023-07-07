using MovieMatchMakerLib;

namespace MovieMatchMakerApp
{
    public partial class MainForm : Form
    {
        private static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string FilePath => Path.Combine(AppDataPath, "moviematchmaker.json");

        private readonly ConnectionManager _connectionManager;
        private readonly IDataCache _dataCache;

        public MainForm()
        {
            _dataCache = JsonFileCache.Load(FilePath);
            var apiDataSource = new ApiDataSource();
            var cachedDataSource = new CachedDataSource(_dataCache, apiDataSource);
            _connectionManager = new ConnectionManager(cachedDataSource);

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UseWaitCursor = true;

            //await _connectionManager.FindMoviesConnectedToMovie("Dark City", 1998, 1);
            //await _dataCache.SaveAsync();            

            UseWaitCursor = false;

            Close();
        }
    }
}