using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
