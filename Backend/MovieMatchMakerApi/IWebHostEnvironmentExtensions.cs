namespace MovieMatchMakerApi
{
    public static class IWebHostEnvironmentExtensions
    {
        public static string MapPath(this IWebHostEnvironment instance, string path)
        {
            return Path.Combine(instance.WebRootPath, path);
        }
    }
}
