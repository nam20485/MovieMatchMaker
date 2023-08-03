using System;

namespace MovieMatchMakerLib.Utils
{
    public class ConsoleEx
    {
        private static ConsoleColor _originalForegroundColor;
        private static ConsoleColor _originalBackgroundColor;        

        public static ConsoleColor ForegroundColor
        {
            set
            {
                _originalForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = value;
            }
        }

        public static ConsoleColor BackgroundColor
        {
            set
            {
                _originalBackgroundColor = Console.BackgroundColor;
                Console.BackgroundColor = value;
            }
        }

        public static void ResetForegroundColor()
        {
            Console.ForegroundColor = _originalForegroundColor;
        }

        public static void ResetBackgroundColor()
        {
            Console.BackgroundColor = _originalBackgroundColor;
        }

        public static void ResetColors()
        {
            ResetForegroundColor();
            ResetBackgroundColor();
        }
    }
}
