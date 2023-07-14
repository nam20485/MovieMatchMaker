using System;

namespace MovieMatchMakerLib.Utils
{
    public class SystemFolders
    {
        public static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
}
