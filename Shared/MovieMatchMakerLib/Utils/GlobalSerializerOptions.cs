using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieMatchMakerLib.Utils
{
    internal class GlobalSerializerOptions
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            // disable pretty-printing in release builds to save file size and xfer speed
            WriteIndented = Macros.IsDebugBuild(),
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            ReferenceHandler = ReferenceHandler.Preserve, 
            PropertyNameCaseInsensitive = true,
        };
    }
}
