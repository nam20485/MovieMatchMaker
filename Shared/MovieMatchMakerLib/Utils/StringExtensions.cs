using System.Linq;


namespace MovieMatchMakerLib.Utils
{
    public static class StringExtensions
    {       
        public static bool ContainsNonAsciiChars(this string str)
        {
            return str.Any(c => !char.IsAscii(c));
        }
    }
}
