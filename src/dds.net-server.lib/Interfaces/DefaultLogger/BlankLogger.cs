namespace DDS.Net.Server.Interfaces.DefaultLogger
{
    /// <summary>
    /// Class <c>BlankLogger</c> implements <c>ILogger</c> interface to discard any log messages.
    /// </summary>
    public class BlankLogger : ILogger
    {
        public void Error(string message)
        {
        }

        public void Info(string message)
        {
        }

        public void Warning(string message)
        {
        }
    }
}
