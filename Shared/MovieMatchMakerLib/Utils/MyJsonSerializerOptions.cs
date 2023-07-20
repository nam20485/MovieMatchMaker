using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieMatchMakerLib.Utils
{
    internal class MyJsonSerializerOptions
    {
        public static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions()
        {
            WriteIndented = Macros.IsDebug(),
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            ReferenceHandler = ReferenceHandler.Preserve, 
            PropertyNameCaseInsensitive = true,
        };
    }
}
