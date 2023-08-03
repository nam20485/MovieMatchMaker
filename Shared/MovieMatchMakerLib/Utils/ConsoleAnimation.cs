using System;
using System.Threading;

namespace MovieMatchMakerLib.Utils
{
    public class ConsoleAnimation : IDisposable
    {
        public delegate string GetFrameTextFunc(ulong frameNumber);

        public const int DefaultDelayMs = 0;

        private readonly GetFrameTextFunc _getFrameTextFunc;
        private readonly Thread _thread;
        private readonly int _delayMs;

        private int _top = -1;
        private int _left = -1;

        private volatile bool _stop = false;

        private ulong _frameNumber;
        private int _longestFrameText;

        public ConsoleAnimation(int left, int top, int delayMs, GetFrameTextFunc getFrameTextFunc)
        {
            _frameNumber = 0;
            _longestFrameText = 0;
            _top = top;
            _left = left;
            _delayMs = delayMs;
            _getFrameTextFunc = getFrameTextFunc;
            _thread = new Thread(DrawFrameLoop);            
        }

        public ConsoleAnimation(int left, int top, GetFrameTextFunc getFrameTextFunc)
            : this(left, top, DefaultDelayMs, getFrameTextFunc)
        {
        }

        public ConsoleAnimation(int delayMs, GetFrameTextFunc getFrameTextFunc)
            : this(-1, -1, delayMs, getFrameTextFunc)
        {
        }

        public ConsoleAnimation(GetFrameTextFunc getFrameTextFunc)
           : this(DefaultDelayMs, getFrameTextFunc)
        {
        }

        public void Dispose()
        {            
            Stop();
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            if (_left == -1 &&
                _top == -1)
            {
                (_left, _top) = Console.GetCursorPosition();
            }
            _thread.Start();
        }

        public void Stop()
        {
            _stop = true;            
            _thread.Join();

            //Console.SetCursorPosition(_left, _top);
            //Console.Write(new string(' ', _longestFrameText));
        }

        private void DrawFrame()
        {
            var ft = _getFrameTextFunc(_frameNumber++);
            Console.SetCursorPosition(_left, _top);                                 
            Console.Write(ft);

            // capture longest frame text length for erasing when Stop()'ing
            if (ft.Length > _longestFrameText)
            {
                _longestFrameText = ft.Length;
            }
        }

        private void DrawFrameLoop()
        {
            while (! _stop)
            {
                DrawFrame();
                Thread.Sleep(_delayMs);
            }
        }
    }
}
