using System.Text.Json;
using System.Text.Json.Serialization;
using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib
{
    public class GlobalSerializerOptions
    {
        public static readonly JsonSerializerOptions Options;

        static GlobalSerializerOptions()
        {
            Options = new ()
            {
                // disable pretty-printing in release builds to save file size and xfer speed
                WriteIndented = Macros.IsDebugBuild(),
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                PropertyNamingPolicy = null,                
                // TODO: why does this need to be on if there is no reference $id metadata in the JSON file?
                //ReferenceHandler = ReferenceHandler.Preserve,                                 
            };
            Options.Converters.Add(new JsonStringEnumConverter());
        }
        
        public static void SetOptions(JsonSerializerOptions options)
        {
            // set options with our global defaults
            options.WriteIndented = Options.WriteIndented;
            options.ReadCommentHandling = Options.ReadCommentHandling;
            options.AllowTrailingCommas =Options.AllowTrailingCommas;
            options.DefaultIgnoreCondition = Options.DefaultIgnoreCondition;
            options.PropertyNamingPolicy = Options.PropertyNamingPolicy;
            options.PropertyNameCaseInsensitive = Options.PropertyNameCaseInsensitive;    
            options.ReferenceHandler = Options.ReferenceHandler;

            // add our converters
            foreach (var converter in Options.Converters)
            {
                if (!options.Converters.Contains(converter))
                {
                    options.Converters.Add(converter);
                }            
            }
        }
    }
}
