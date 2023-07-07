using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatchMakerLib
{
    public class Auth
    {
        private const string TMDB_API_KEY_ENV_NAME = "TMDB_API_KEY";
        public static string GetTmdbApiKey() => Environment.GetEnvironmentVariable(TMDB_API_KEY_ENV_NAME);
    }
}
