using System;
using System.IO;

namespace BuildChromium.Logging
{
    public sealed class FileLogger : ILogWriter, IDisposable
    {
        public string LogFormat { get; set; } = "{time} - {level}: {message}";

        private StreamWriter writer;
        public FileLogger(FileInfo file)
        {
            Stream stream = file.Open(FileMode.Create);
            writer = new StreamWriter(stream);
        }

        public LogLevel MinimumLevel { get; set; }

        public void Dispose()
        {
            writer.Dispose();
        }

        public void Write(LogLevel level, string message)
        {
            string line = LogFormat;
            line = line.Replace("{time}", DateTime.Now.ToString());
            line = line.Replace("{level}", level.ToString());
            line = line.Replace("{message}", message);

            writer.WriteLine(line);
        }
    }
}
