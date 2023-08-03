using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieMatchMakerLib.Utils
{
    internal class GlobalSerializerOptions
    {
        internal static readonly JsonSerializerOptions Options;

        static GlobalSerializerOptions()
        {
            Options = new()
            {
                // disable pretty-printing in release builds to save file size and xfer speed
                WriteIndented = Macros.IsDebugBuild(),
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                // TODO: why does this need to be on if there is no reference $id metadata in the JSON file?
                ReferenceHandler = ReferenceHandler.Preserve, 
                //PropertyNameCaseInsensitive = true,                
            };
            Options.Converters.Add(new JsonStringEnumConverter());
        }        
    }
}
