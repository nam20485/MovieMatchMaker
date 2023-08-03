using System;
using System.Threading;

namespace MovieMatchMakerLib.Utils
{
    public class ConsoleAnimation : IDisposable
    {
        public delegate string GetFrameTextFunc(ulong frameNumber);

        private readonly GetFrameTextFunc _getFrameTextFunc;
        private readonly Thread _thread;
        private readonly int _delayMs = 0;

        private int _top = -1;
        private int _left = -1;

        private bool _stop = false;

        private ulong _frameNumber;
        private string _lastFrameText;

        public ConsoleAnimation(int left, int top, GetFrameTextFunc getFrameTextFunc)
        {
            _frameNumber = 0;
            _lastFrameText = "";
            _top = top;
            _left = left;
            _getFrameTextFunc = getFrameTextFunc;
            _thread = new Thread(DrawFrameLoop);          
        }

        public ConsoleAnimation(GetFrameTextFunc getFrameTextFunc)
            : this(-1, -1, getFrameTextFunc)
        {
        }

        public void Dispose()
        {            
            Stop();
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            if (_left == -1 && _top == -1)
            {
                (_left, _top) = Console.GetCursorPosition();
            }
            _thread.Start();
        }

        public void Stop()
        {
            _stop = true;
            _thread.Join();
        }

        private void DrawFrame()
        {
            _lastFrameText = _getFrameTextFunc(_frameNumber++);
            Console.SetCursorPosition(_left, _top);                                 
            Console.Write(_lastFrameText);
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
