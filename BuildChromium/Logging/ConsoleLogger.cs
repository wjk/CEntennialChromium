using System;

namespace BuildChromium.Logging
{
    public sealed class ConsoleLogger : ILogWriter
    {
        public string LogFormat { get; set; } = "{time} - {level}: {message}";

        public LogLevel MinimumLevel { get; set; } = LogLevel.Verbose;

        public void Write(LogLevel level, string message)
        {
            string line = LogFormat;
            line = line.Replace("{time}", DateTime.Now.ToString());
            line = line.Replace("{level}", level.ToString());
            line = line.Replace("{message}", message);

            switch (level)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Informational:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.FatalError:
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
            }

            if (level >= LogLevel.Error) Console.Error.WriteLine(line);
            else Console.WriteLine(line);

            Console.ResetColor();
        }
    }
}
