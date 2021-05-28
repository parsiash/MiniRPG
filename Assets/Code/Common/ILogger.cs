namespace MiniRPG.Common
{
    /// <summary>
    /// The abstraction for any logger used in the project.
    /// </summary>
    public interface ILogger
    {
        void Log(LogType logType, string message);
    }

    public static class ILoggerExtensions
    {
        public static void Log(this ILogger logger, string message)
        {
            logger.Log(LogType.Info, message);
        }

        public static void LogError(this ILogger logger, string message)
        {
            logger.Log(LogType.Error, message);
        }
    }


    public enum LogType
    {
        Info,
        Error,
        Debug,
        Warning,
        Trace
    }
}
