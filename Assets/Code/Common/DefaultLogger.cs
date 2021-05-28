namespace MiniRPG.Common
{
    /// <summary>
    /// A logger implemented using Unity Log System.
    /// </summary>
    public class DefaultLogger : Singleton<DefaultLogger>, ILogger
    {
        public void Log(LogType logType, string message)
        {
            switch(logType)
            {
                case LogType.Info:
                    UnityEngine.Debug.Log(message);
                    break;

                case LogType.Error:
                    UnityEngine.Debug.LogError(message);
                    break;

                default:
                    UnityEngine.Debug.Log(message);
                    break;
            }
        }
    }
}
