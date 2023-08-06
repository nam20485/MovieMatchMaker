using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public class MmmCliArguments : CliArguments
    {
        public bool Timing => GetPropertyArgumentValue<bool>();
        public bool BuildData => GetPropertyArgumentValue<bool>();
        public bool BuildConnections => GetPropertyArgumentValue<bool>();
        public bool Threaded => GetPropertyArgumentValue<bool>();
        public bool ContinueExisting => GetPropertyArgumentValue<bool>();
        public string File => GetPropertyArgumentValue<string>();
        public string Title => GetPropertyArgumentValue<string>();
        public int ReleaseYear => GetPropertyArgumentValue<int>();
        public int Degree => GetPropertyArgumentValue<int>();
        public Logger.Level LogLevel => GetPropertyArgumentValue<Logger.Level>();

        //--build-data --title "Dark City" --releaseYear 1998 --degree 1 --threaded true --file ./movie-data.json --continue false

        public MmmCliArguments(string[] args) 
            : base(args)
        {            
        }      
    }
}
