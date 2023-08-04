using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class EllipsisSequenceAnimation : SequenceAnimation
    {
        private static string[] _ellipsisSequence = new[] { ".", "..", "...", "..", "." };


        public EllipsisSequenceAnimation()
            : base(_ellipsisSequence)
        {
        }

        public EllipsisSequenceAnimation(int delayMs) : base(delayMs, _ellipsisSequence)
        {
        }

        public EllipsisSequenceAnimation(int left, int top) : base(left, top, _ellipsisSequence)
        {
        }

        public EllipsisSequenceAnimation(int left, int top, int delayMs) : base(left, top, delayMs, _ellipsisSequence)
        {
        }
    }
}
