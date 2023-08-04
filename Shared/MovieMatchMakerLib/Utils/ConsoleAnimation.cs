using System;
using System.Threading;

namespace MovieMatchMakerLib.Utils
{
    public class ConsoleAnimation : IDisposable
    {
        public delegate string GetFrameTextFunc(ulong frameNumber);

        private const int DefaultDelayMs = 0;

        public int Top { get; private set; }
        public int Left { get; private set; }

        public int DelayMs {  get; set; }
        public bool HideCursor { get; set; }
        public bool Clear { get; set; }
        public string LastFrame { get; set; }
        
        public ConsoleColor ForegroundColor
        {
            get
            {
                return Console.ForegroundColor;
            }
            set
            {
                _consoleColors.ForegroundColor = value;
            }
        }
        public ConsoleColor BackgroundColor
        {
            get
            {
                return Console.BackgroundColor;
            }
            set
            {
                _consoleColors.BackgroundColor = value;
            }
        }

        private ConsoleColors _consoleColors;

        protected GetFrameTextFunc _getFrameTextFunc;
        private readonly Thread _thread;             

        private volatile bool _stop = false;
        private ulong _frameNumber;
        private int _longestFrameText;

        public ConsoleAnimation(int left, int top, int delayMs, GetFrameTextFunc getFrameTextFunc)
        {
            _getFrameTextFunc = getFrameTextFunc;
            _thread = new Thread(DrawFrameLoop);
            _frameNumber = 0;
            _longestFrameText = 0;
            _consoleColors = new ConsoleColors();

            Top = top;
            Left = left;
            DelayMs = delayMs;         
            HideCursor = true;
            Clear = false;

            ForegroundColor = Console.ForegroundColor;
            BackgroundColor = Console.BackgroundColor;                    
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

        private string LongestFrameLengthBlank() => new (' ', _longestFrameText);

        public void Dispose()
        {            
            Stop();
            _consoleColors.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            if (HideCursor)
            {
                Console.CursorVisible = false;
            }
            if (Left == -1 &&
                Top == -1)
            {
                (Left, Top) = Console.GetCursorPosition();
            }
            _thread.Start();
        }

        private (int Left, int Top) _saved = Console.GetCursorPosition();

        private void SavePosition()
        {
            _saved = Console.GetCursorPosition();
        }

        public void RestPosition()
        {
            Console.SetCursorPosition(_saved.Left, _saved.Top);
        }

        public void Stop()
        {
            _stop = true;            
            _thread.Join();

            SavePosition();

            if (Clear)
            {
                EraseFrame();
            }

            if (!string.IsNullOrWhiteSpace(LastFrame))
            {
                EraseFrame();
                DrawFrame(LastFrame);
            }

            RestPosition();
            
            if (HideCursor)
            {
                Console.CursorVisible = true;
            }
        }

        private void EraseFrame()
        {            
            DrawFrame(LongestFrameLengthBlank());            
        }

        private void DrawFrame(string frameText)
        {
            Console.Write(LongestFrameLengthBlank());
            Console.SetCursorPosition(Left, Top);                                 
            Console.Write(frameText);            
        }

        private void DrawFrameLoop()
        {
            while (! _stop)
            {
                var ft = _getFrameTextFunc(_frameNumber++);
                DrawFrame(ft);
                // capture longest frame text length for erasing when Stop()'ing
                if (ft.Length > _longestFrameText)
                {
                    _longestFrameText = ft.Length;
                }
                Thread.Sleep(DelayMs);
            }
        }
    }
}
