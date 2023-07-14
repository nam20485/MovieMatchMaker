using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MovieMatchMakerLib.Utils
{
    public class SystemFolders
    {
        public static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
}
