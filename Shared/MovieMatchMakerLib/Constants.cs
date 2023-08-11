namespace MovieMatchMakerLib
{
    public static class Constants
    {
        public static class Strings
        {
            public const string Title = "Movie Match Maker";
            public const string Slogan = "Find movies you didn't know you liked!";
            public readonly static string Header = $"{Title} - {Slogan}";
            public const string Version = "0.1";
            public readonly static string HeaderWithVersion = $"{Title} v{Version} - {Slogan}";
        }
        public static class Regex
        {
            public const string AsciiLettersAndNumbersOnly = "^[a-zA-Z0-9]+$";
            public const string UnicodeLettersAndNumbersOnly = "[\\p{L}\\p{N}]";
            public const string UnicodeLettersNumbersAndSpaceOnly = "[ \\p{L}\\p{N}]";
        }
    }
}
