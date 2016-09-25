using System;

namespace OgameBot.Logging
{
    public class Logger
    {
        public static Logger Instance { get; } = new Logger();

        private Logger()
        {

        }

        public void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("DBG");
                    Console.ResetColor();
                    Console.Write("] ");
                    break;
                case LogLevel.Info:
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("INF");
                    Console.ResetColor();
                    Console.Write("] ");
                    break;
                case LogLevel.Warning:
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("WRN");
                    Console.ResetColor();
                    Console.Write("] ");
                    break;
                case LogLevel.Error:
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("ERR");
                    Console.ResetColor();
                    Console.Write("] ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            Console.WriteLine(message);
        }

        public void LogException(Exception ex, string message = null)
        {
            Log(LogLevel.Error, $"Exception: {ex.Message}");
            Log(LogLevel.Error, $"Type: {ex.GetType().FullName}");

            if (string.IsNullOrEmpty(message))
                Log(LogLevel.Error, $"Message: {message}");

            Log(LogLevel.Error, ex.StackTrace);
            Log(LogLevel.Error, string.Empty);
        }
    }
}