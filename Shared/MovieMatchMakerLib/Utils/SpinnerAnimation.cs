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

        public SpinnerAnimation(int delayMs)
           : base(delayMs, GetSpinnerFrameText)
        {
        }

        public SpinnerAnimation(int left, int top, int delayMs)
            : base(left, top, delayMs, GetSpinnerFrameText)
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
