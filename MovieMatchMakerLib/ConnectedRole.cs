using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace MovieMatchMakerLib
{
    public class ConnectedRole
    {
        public Name Name { get; set; }
        public int PersonId { get; set; }
        public string TargetJob { get; set; }
        public string SourceJob { get; set; }

        public class StringDictionary : Dictionary<string, ConnectedRole>
        {
        }

        public class List : List<ConnectedRole>
        {
        }      
    }
}
