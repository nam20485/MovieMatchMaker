namespace MovieMatchMakerApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        public static string? AccessToken => Environment.GetEnvironmentVariable("TMDB_ACCESS_TOKEN");
        public static string? ApiKey => Environment.GetEnvironmentVariable("TMDB_API_KEY");


    }
}