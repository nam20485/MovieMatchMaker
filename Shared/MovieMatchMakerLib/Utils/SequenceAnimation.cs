using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class SequenceAnimation : ConsoleAnimation
    {
        public string[] Sequence { get; set; }

        public SequenceAnimation(string[] sequence)
            : base(null)
        {
            Sequence = sequence;
            _getFrameTextFunc = GetFrameText;
        }

        public SequenceAnimation(int delayMs, string[] sequence)
            : base(delayMs, null)
        {
            _getFrameTextFunc = GetFrameText;
            Sequence = sequence;
        }

        public SequenceAnimation(int left, int top, string[] sequence)
            : base(left, top, null)
        {
            _getFrameTextFunc = GetFrameText;
            Sequence = sequence;
        }

        public SequenceAnimation(int left, int top, int delayMs, string[] sequence)
            : base(left, top, delayMs, null)
        {
            _getFrameTextFunc = GetFrameText;
            Sequence = sequence;
        }

        private string GetFrameText(ulong frameNumber)
        {
            return Sequence[frameNumber % (ulong)Sequence.Length];
        }
    }
}
