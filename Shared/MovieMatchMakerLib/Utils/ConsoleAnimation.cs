using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public class ConsoleAnimation : IDisposable
    {
        private readonly Func<string> _getFrameTextFunc;
        private readonly Thread _thread;
        private readonly int _delayMs = 0;

        private int _top = -1;
        private int _left = -1;

        private bool _stop = false;

        public ConsoleAnimation(int left, int top, Func<string> getFrameTextFunc)
        {
            _top = top;
            _left = left;
            _getFrameTextFunc = getFrameTextFunc;
            _thread = new Thread(DrawFrameLoop);          
        }

        public ConsoleAnimation(Func<string> getFrameTextFunc)
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
            var s = _getFrameTextFunc();
            Console.SetCursorPosition(_left, _top);                                 
            Console.Write(s);
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
