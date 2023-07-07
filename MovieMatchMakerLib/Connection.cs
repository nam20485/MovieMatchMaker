using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatchMakerLib
{
    public class Connection
    {
        public Movie SourceMovie { get; set; }
        public Movie TargetMovie { get; set; }
        public List<ProductionMember> ProductionMembers { get; set; }

    }
}
