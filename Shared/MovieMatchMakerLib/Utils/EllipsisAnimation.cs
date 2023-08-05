namespace MovieMatchMakerLib.Utils
{
    public class EllipsisAnimation : SequenceAnimation
    {
        protected static readonly string[] _ellipsisSequence = { ".", "..", "...", "..", "." };
        //private static readonly string[] _ellipsisSequence = { ".", "..", "...", "   " };        

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
