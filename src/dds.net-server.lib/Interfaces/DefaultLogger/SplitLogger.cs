namespace DDS.Net.Server.Interfaces.DefaultLogger
{
    public class SplitLogger : ILogger, IDisposable
    {
        private readonly ILogger firstLogger;
        private readonly ILogger secondLogger;
        private readonly ILogger[]? loggers;

        public SplitLogger(ILogger firstLogger, ILogger secondLogger, params ILogger[] loggers)
        {
            this.firstLogger = firstLogger ?? throw new ArgumentNullException(nameof(firstLogger));
            this.secondLogger = secondLogger ?? throw new ArgumentNullException(nameof(secondLogger));
            this.loggers = loggers;
        }

        public void Dispose()
        {
            if (firstLogger is IDisposable firstDisposable) { firstDisposable.Dispose(); }
            if (secondLogger is IDisposable secondDisposable) { secondDisposable.Dispose(); }

            if (loggers != null && loggers.Length > 0)
            {
                foreach (var logger in loggers)
                {
                    if (logger != null && logger is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }

        public void Info(string message)
        {
            firstLogger?.Info(message);
            secondLogger?.Info(message);

            if (loggers != null && loggers.Length > 0)
            {
                foreach (var logger in loggers)
                {
                    logger?.Info(message);
                }
            }
        }

        public void Warning(string message)
        {
            firstLogger?.Warning(message);
            secondLogger?.Warning(message);

            if (loggers != null && loggers.Length > 0)
            {
                foreach (var logger in loggers)
                {
                    logger?.Warning(message);
                }
            }
        }

        public void Error(string message)
        {
            firstLogger.Error(message);
            secondLogger.Error(message);

            if (loggers != null && loggers.Length > 0)
            {
                foreach (var logger in loggers)
                {
                    logger?.Error(message);
                }
            }
        }
    }
}
