using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class EllipsisAnimation : ConsoleAnimation
    {
        private const int DelayMs = 200;

        public EllipsisAnimation()
                : base(DelayMs, GetFrameText)
        {
        }      

        public EllipsisAnimation(int left, int top)
            : base(left, top, DelayMs, GetFrameText)
        {
        }

        private static string GetFrameText(ulong frameNumber)
        {
            return (frameNumber % 4) switch
            {
                0 => ".",
                1 => "..",
                2 => "...",
                //3 => "   ",
                3 => "..",                
            };
        }
    }
}
