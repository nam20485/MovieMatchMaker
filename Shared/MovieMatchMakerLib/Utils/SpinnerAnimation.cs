using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class SpinnerAnimation : ConsoleAnimation
    {
        public SpinnerAnimation()
            : base(GetSpinnerFrameText)
        {
        }

        public SpinnerAnimation(int left, int top)
            : base(left, top, GetSpinnerFrameText)
        {
        }

        private static string GetSpinnerFrameText(ulong frameNumber)
        {
            return (frameNumber % 4) switch
            {
                0 => "/",
                1 => "-",
                2 => @"\",
                3 => "|",
            };
        }
    }
}
