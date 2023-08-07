using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public class MmmCliArguments : CliArguments
    {
        public bool Timing => GetArgumentValueForCallableName<bool>();
        public bool BuildData => GetArgumentValueForCallableName<bool>();
        public bool BuildConnections => GetArgumentValueForCallableName<bool>();
        public bool SingleThreaded => GetArgumentValueForCallableName<bool>();
        public bool ContinueExisting => GetArgumentValueForCallableName<bool>();
        public string File => GetArgumentValueForCallableName<string>();
        public string Title => GetArgumentValueForCallableName<string>();
        public int ReleaseYear => GetArgumentValueForCallableName<int>();
        public int Degree => GetArgumentValueForCallableName<int>();
        public Logger.Level LogLevel => GetArgumentValueForCallableName<Logger.Level>();

        //--build-data --title "Dark City" --releaseYear 1998 --degree 1 --threaded true --file ./movie-data.json --continue false

        public MmmCliArguments(string[] args) 
            : base(args)
        {            
        }      
    }
}
