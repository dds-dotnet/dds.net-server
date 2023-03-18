namespace DDS.Net.Server.Interfaces
{
    /// <summary>
    /// Provides selection of minimum level of log messages to reach output by
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
        /// Outputs <c>Information</c>-level (<c cref="LogLevel.Information">LogLevel.Information</c>) log message.
        /// </summary>
        /// <param name="message">The message text.</param>
        void Info(string message);
        /// <summary>
        /// Outputs <c>Warning</c>-level (<c cref="LogLevel.Warning">LogLevel.Warning</c>) log message.
        /// </summary>
        /// <param name="message">The message text.</param>
        void Warning(string message);
        /// <summary>
        /// Outputs <c>Error</c>-level (<c cref="LogLevel.Error">LogLevel.Error</c>) log message.
        /// </summary>
        /// <param name="message">The message text.</param>
        void Error(string message);
    }
}
