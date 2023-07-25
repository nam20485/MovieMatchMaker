using System;

namespace MovieMatchMakerLib.Utils
{
    public class SystemFolders
    {
        public static string LocalAppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
}
