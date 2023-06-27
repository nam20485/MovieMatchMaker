using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib
{
    public class Production
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }     

        public Director Director { get; set; }     
        public List<Writer> Writers { get; set; }
        public List<Actor> Actors { get; set; }

        public Production()
        {            
            Writers = new List<Writer>();
            Actors = new List<Actor>();
        }  
        
        public Production(string title, int releaseYear)
            : this()
        {
            Title = title;
            ReleaseYear = releaseYear;
        }

        public static Production FetchMetadata(string title, int releaseYear)
        {
            return null;
        }
    }
}
