using System;

namespace MovieMatchMakerLib.Utils
{
    public static class Macros
    {
        public static bool IsDebugBuild()
        {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
        }

        public static bool IsReleaseBuild()
        {
            return ! IsDebugBuild();
        }

        private const string CI_ENV_VAR_NAME = "CI";

        public static bool IsCI()
        {
            return Environment.GetEnvironmentVariable(CI_ENV_VAR_NAME) is not null;
        }
    }
}
