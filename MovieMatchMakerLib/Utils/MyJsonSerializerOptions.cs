using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MovieMatchMakerLib.Utils
{
    internal class MyJsonSerializerOptions
    {
        public static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions()
        {
            WriteIndented = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            //ReferenceHandler = ReferenceHandler.Preserve,
        };
    }
}
