using DDS.Net.Server.Interfaces;
using DDS.Net.Server.PublicExtensions;

namespace DDS.Net.Server.ConsoleApp.Interfaces.Logger
{
    internal class FileLogger : ILogger, IDisposable
    {
        private readonly LogLevel _logLevel;
        private StreamWriter? _writer;

        public FileLogger(string filename, LogLevel logLevel = LogLevel.Information)
        {
            try
            {
                filename.CreateFoldersForRelativeFilename();

                _writer = File.AppendText(filename);

                _writer.WriteLine($"╔═══════════════════════════════════════════════════════════════════╗");
                _writer.WriteLine($"║ DDS.Net Server                                                    ║");
                _writer.WriteLine($"║-------------------------------------------------------------------║");
                _writer.WriteLine($"║                                                                   ║");
                _writer.WriteLine($"║ Starting log @                                                    ║");
                _writer.WriteLine($"║     Local time: {DateTime.Now,-35}               ║");
                _writer.WriteLine($"║     UTC time:   {DateTime.UtcNow,-35}               ║");
                _writer.WriteLine($"║                                                                   ║");
                _writer.WriteLine($"╚═══════════════════════════════════════════════════════════════════╝");

                _writer.Flush();
                _writer.AutoFlush = true;
            }
            catch (Exception ex)
            {
                _writer = null;
                throw new Exception($"Cannot write logfile \"{filename}\" - {ex.Message}");
            }

            _logLevel = logLevel;
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.WriteLine($"╔═══════════════════════════════════════════════════════════════════╗");
                _writer.WriteLine($"║ Stopping log @                                                    ║");
                _writer.WriteLine($"║     Local time: {DateTime.Now,-35}               ║");
                _writer.WriteLine($"║     UTC time:   {DateTime.UtcNow,-35}               ║");
                _writer.WriteLine($"╚═══════════════════════════════════════════════════════════════════╝");
                _writer.WriteLine($"");
                _writer.WriteLine($"");
                _writer.WriteLine($"");

                _writer.Flush();
                _writer.Dispose();
                _writer = null;
            }
        }

        public void Error(string message)
        {
            lock (this)
            {
                _writer?.WriteLine($"Error: {message}");
            }
        }

        public void Info(string message)
        {
            if (_logLevel == LogLevel.Information)
            {
                lock (this)
                {
                    _writer?.WriteLine(message);
                }
            }
        }

        public void Warning(string message)
        {
            if (_logLevel != LogLevel.Error)
            {
                lock (this)
                {
                    _writer?.WriteLine($"Warning: {message}");
                }
            }
        }
    }
}
