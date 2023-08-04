namespace MovieMatchMakerLib.Utils
{
    public class SequenceAnimation : ConsoleAnimation
    {
        public string[] Sequence { get; set; }

        public SequenceAnimation(string[] sequence)  
            : base(null)
        {
            Sequence = sequence;
            FrameDelayMs = 200;
        }       

        public SequenceAnimation(int left, int top, string[] sequence)
            : base(left, top, null)
        {
            Sequence = sequence;
            FrameDelayMs = 200;
        }        

        protected override string GetFrameText(uint frameNumber)
        {
            return Sequence[frameNumber % Sequence.Length];
        }
    }
}
