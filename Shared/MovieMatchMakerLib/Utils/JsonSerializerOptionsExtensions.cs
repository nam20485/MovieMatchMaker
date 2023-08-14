using System.Text.Json;

namespace MovieMatchMakerLib.Utils
{
    public static class JsonSerializerOptionsExtensions
    {              
        public static void SetFrom(this JsonSerializerOptions instance, JsonSerializerOptions from)
        {
            // set options with our global defaults
            instance.WriteIndented = from.WriteIndented;
            instance.ReadCommentHandling = from.ReadCommentHandling;
            instance.AllowTrailingCommas = from.AllowTrailingCommas;
            instance.DefaultIgnoreCondition = from.DefaultIgnoreCondition;
            instance.PropertyNamingPolicy = from.PropertyNamingPolicy;
            instance.PropertyNameCaseInsensitive = from.PropertyNameCaseInsensitive;
            instance.ReferenceHandler = from.ReferenceHandler;

            // add our converters
            foreach (var converter in from.Converters)
            {
                if (!instance.Converters.Contains(converter))
                {
                    instance.Converters.Add(converter);
                }
            }
        }
    }
}
