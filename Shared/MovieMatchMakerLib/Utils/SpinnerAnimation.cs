namespace MovieMatchMakerLib.Utils
{
    public class SpinnerAnimation : SequenceAnimation
    {
        protected static readonly string[] _spinnerSequence = { "/", "-", "\\" };

        public SpinnerAnimation()
            : base(_spinnerSequence)
        {            
        }       

        public SpinnerAnimation(int left, int top)
            : base(left, top, _spinnerSequence)
        {            
        }        
    }
}
