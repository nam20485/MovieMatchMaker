using System;
using System.ComponentModel.DataAnnotations;

namespace MovieMatchMakerLib.Utils
{
    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    //                AllowMultiple = false)]
    public class YearRangeAttribute : RangeAttribute
    {
        /// <summary>
        /// One parameter ctor specifies range: [minimum, DateTime.UtcNow.Year] (inclusive), i.e. up to current year
        /// </summary>
        /// <param name="minimum">minimum year, beginning of range (inclusive)</param>
        public YearRangeAttribute(int minimum)
            : base(minimum, DateTime.UtcNow.Year)
        {
        }

        /// <summary>
        /// No parameter ctor specifies range: [DateTime.MinValue.Year, DateTime.UtcNow.Year] (inclusive)
        /// </summary>
        public YearRangeAttribute()
            : base(DateTime.MinValue.Year, DateTime.UtcNow.Year)
        {
        }

        public YearRangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        {
        }

        // not sure what this is for- disable it until its found to be needed
        //public YearRangeAttribute([DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] Type type,
        //                          string minimum,
        //                          string maximum)
        //    : base(type, minimum, maximum)
        //{
        //}
    }
}
