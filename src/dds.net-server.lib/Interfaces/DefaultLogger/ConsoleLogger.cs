using DDS.Net.Server.PublicExtensions;

namespace DDS.Net.Server.Interfaces.DefaultLogger
{
    /// <summary>
    /// Class <c>ConsoleLogger</c> implements <c>ILogger</c> interface to output log messages
    /// to standard console.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private readonly LogLevel logLevel;

        private readonly ConsoleColor informationTextColor;
        private readonly ConsoleColor informationBackgroundColor;
        private readonly ConsoleColor warningTextColor;
        private readonly ConsoleColor warningBackgroundColor;
        private readonly ConsoleColor errorTextColor;
        private readonly ConsoleColor errorBackgroundColor;

        /// <summary>
        /// Initializes the class with minimum log-level and colors for output log messages.
        /// </summary>
        /// <param name="logLevel">Minimum log-level.</param>
        /// <param name="informationTextColor">Text color for <c>Information</c>-level messages.</param>
        /// <param name="informationBackgroundColor">Background color for <c>Information</c>-level messages.</param>
        /// <param name="warningTextColor">Text color for <c>Warning</c>-level messages.</param>
        /// <param name="warningBackgroundColor">Background color for <c>Warning</c>-level messages.</param>
        /// <param name="errorTextColor">Text color for <c>Error</c>-level messages.</param>
        /// <param name="errorBackgroundColor">Background color for <c>Error</c>-level messages.</param>
        public ConsoleLogger(
            LogLevel logLevel = LogLevel.Information,

            ConsoleColor informationTextColor = ConsoleColor.DarkGray,
            ConsoleColor informationBackgroundColor = ConsoleColor.Black,
            ConsoleColor warningTextColor = ConsoleColor.Yellow,
            ConsoleColor warningBackgroundColor = ConsoleColor.Black,
            ConsoleColor errorTextColor = ConsoleColor.Magenta,
            ConsoleColor errorBackgroundColor = ConsoleColor.Black)
        {
            this.logLevel = logLevel;

            this.informationTextColor = informationTextColor;
            this.informationBackgroundColor = informationBackgroundColor;
            this.warningTextColor = warningTextColor;
            this.warningBackgroundColor = warningBackgroundColor;
            this.errorTextColor = errorTextColor;
            this.errorBackgroundColor = errorBackgroundColor;
        }

        public void Info(string message)
        {
            if (logLevel == LogLevel.Information)
            {
                lock (this)
                {
                    message.WriteLine(informationTextColor, informationBackgroundColor);
                }
            }
        }

        public void Warning(string message)
        {
            if (logLevel != LogLevel.Error)
            {
                lock (this)
                {
                    $"Warning: {message}".WriteLine(warningTextColor, warningBackgroundColor);
                }
            }
        }

        public void Error(string message)
        {
            lock (this)
            {
                $"Error: {message}".WriteLine(errorTextColor, errorBackgroundColor);
            }
        }
    }
}
