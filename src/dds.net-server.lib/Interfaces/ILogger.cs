namespace DDS.Net.Server.Interfaces
{
    /// <summary>
    /// Option to select the minimum level of log messages to reach output by
    /// any implementation of <c cref="ILogger">ILogger</c>.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Identifies <c>Information</c>-level log messages.
        /// </summary>
        Information,
        /// <summary>
        /// Identifies <c>Warning</c>-level log messages.
        /// </summary>
        Warning,
        /// <summary>
        /// Identifies <c>Error</c>-level log messages.
        /// </summary>
        Error
    }

    /// <summary>
    /// The interface <c>ILogger</c> provides means to output log messages
    /// by different parts of the application.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Outputs <c>Information</c>-level log message.
        /// </summary>
        /// <param name="message">The message text.</param>
        void Info(string message);
        /// <summary>
        /// Outputs <c>Warning</c>-level log message.
        /// </summary>
        /// <param name="message">The message text.</param>
        void Warning(string message);
        /// <summary>
        /// Outputs <c>Error</c>-level log message.
        /// </summary>
        /// <param name="message">The message text.</param>
        void Error(string message);
    }
}
