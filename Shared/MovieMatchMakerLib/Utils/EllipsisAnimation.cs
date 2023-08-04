using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class EllipsisAnimation : SequenceAnimation
    {
        private static readonly string[] _ellipsisSequence = { ".", "..", "...", "..", "." };
        //private static readonly string[] _ellipsisSequence = { ".", "..", "...", "   " };

        //public bool EndWithBlank { get; set; }

        public EllipsisAnimation()
            : base(_ellipsisSequence)
        {            
        }
       
        public EllipsisAnimation(int left, int top)
            : base(left, top, _ellipsisSequence)
        {        
        }
    }
}
