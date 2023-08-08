using System;

namespace MovieMatchMakerLib.TmdbApi
{
    public class TmdbApiHelper
    {
        private const string TMDB_API_KEY_ENV_NAME = "TMDB_API_KEY";
        private const string TMDB_ACCESS_TOKEN_ENV_NAME = "TMDB_ACCESS_TOKEN";

        public const string TMDB_URL = "https://www.themoviedb.org";
        public const string TMDB_IMAGE_BASE_PATH = "https://image.tmdb.org/t/p";        

        public static string TmdbApiKey => Environment.GetEnvironmentVariable(TMDB_API_KEY_ENV_NAME);
        public static string TmdbAccessToken => Environment.GetEnvironmentVariable(TMDB_ACCESS_TOKEN_ENV_NAME);

        public static string MakeTmdbUrl(string type, int id)
        {
            return $"{TMDB_URL}/{type}/{id}";
        }

        public enum PosterImageSize
        {
            w92,
            w154,
            w185,
            w342,
            w500,
            w780,
            original
        }

        public static string MakeImagePath(PosterImageSize imageSize, string imagePath)
        {
            return $"{TMDB_IMAGE_BASE_PATH}/{imageSize}{imagePath}";
        }
    }
}
