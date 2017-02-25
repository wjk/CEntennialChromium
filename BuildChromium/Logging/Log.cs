using System.Collections.Generic;

namespace BuildChromium.Logging
{
    public static class Log
    {
        private static readonly HashSet<ILogWriter> Writers = new HashSet<ILogWriter>();
        public static bool AddLogger(ILogWriter writer) => Writers.Add(writer);
        public static bool RemoveLogger(ILogWriter writer) => Writers.Remove(writer);

        public static void Write(LogLevel level, string format, params object[] args)
        {
            foreach (var writer in Writers)
            {
                if (level > writer.MinimumLevel) continue;
                writer.Write(level, string.Format(format, args));
            }
        }
    }
}
