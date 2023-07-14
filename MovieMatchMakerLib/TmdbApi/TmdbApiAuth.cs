using System;

namespace MovieMatchMakerLib.TmdbApi
{
    public class TmdbApiAuth
    {
        private const string TMDB_API_KEY_ENV_NAME = "TMDB_API_KEY";
        private const string TMDB_ACCESS_TOKEN_ENV_NAME = "TMDB_ACCESS_TOKEN";

        public static string TmdbApiKey => Environment.GetEnvironmentVariable(TMDB_API_KEY_ENV_NAME);
        public static string TmdbAccessToken => Environment.GetEnvironmentVariable(TMDB_ACCESS_TOKEN_ENV_NAME);


    }
}
