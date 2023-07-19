using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Filters
{
    public class TargetReleaseYearRangeFilter : MovieConnectionListFilterBase
    {        
        public int FromYear { get; }
        public int ToYear { get; }

        public bool Inclusive { get; set; }

        public TargetReleaseYearRangeFilter(int fromYear, int toYear)
            : this(fromYear, toYear, true)
        {          
            // default to Inclusive = true
        }

        public TargetReleaseYearRangeFilter(int fromYear, int toYear, bool inclusive)                        
        {
            FromYear = fromYear;
            ToYear = toYear;
            Inclusive = inclusive;
        }            

        protected override MovieConnection.List FilterList(MovieConnection.List list)
        {
            return new MovieConnection.List(
                list.FindAll(mc =>
                {
                    if (Inclusive)
                    {
                        return FromYear <= mc.TargetMovie.ReleaseYear && mc.TargetMovie.ReleaseYear <= ToYear;
                    }
                    else
                    {
                        return FromYear < mc.TargetMovie.ReleaseYear && mc.TargetMovie.ReleaseYear < ToYear;
                    }
                }));
        }
    }
}
