using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace MovieMatchMakerLib
{
    public class MovieConnection
    {
        public Movie SourceMovie { get; set; }
        public Movie TargetMovie { get; set; }  
        
        public ConnectedRole.List ConnectedRoles {  get; set; }

        public MovieConnection(Movie sourceMovie, Movie targetMovie)
        {
            SourceMovie = sourceMovie;
            TargetMovie = targetMovie;
            ConnectedRoles = new ConnectedRole.List();
        }

        public class StringDictionary : Dictionary<string, MovieConnection>
        {            
        }

        public class List : List<MovieConnection>
        {
            public List()
                : base()
            {
            }

            public List(IEnumerable<MovieConnection> collection)
                : base(collection)
            {
            }    
            
            public string ToJson()
            {
                return JsonSerializer.Serialize(this, MyJsonSerializerOptions.JsonSerializerOptions);
            }

            public static List FromJson(string json)
            {
                return JsonSerializer.Deserialize<List>(json, MyJsonSerializerOptions.JsonSerializerOptions);
            }

            public void SaveToFile(string path)
            {
                File.WriteAllText(ToJson(), path);
            }

            public static List LoadFromFile(string path)
            {
                return FromJson(File.ReadAllText(path));
            }
        }
    }
}
