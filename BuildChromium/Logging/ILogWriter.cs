namespace BuildChromium.Logging
{
    public interface ILogWriter
    {
        void Write(LogLevel level, string message);
        LogLevel MinimumLevel { get; set; }
    }

    public enum LogLevel
    {
        FatalError,
        Error,
        Warning,
        Informational,
        Verbose,
        Debug
    }
}
