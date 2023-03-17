using DDS.Net.Server.PublicExtensions;

namespace DDS.Net.Server.Interfaces.DefaultLogger
{
    public class FileLogger : ILogger, IDisposable
    {
        private readonly LogLevel _logLevel;
        private readonly bool _timestamp;
        private StreamWriter? _writer;

        public FileLogger(string filename, LogLevel logLevel = LogLevel.Information, bool timestamp = true)
        {
            try
            {
                filename.CreateFoldersForRelativeFilename();

                _writer = File.AppendText(filename);

                _writer.WriteLine($"DDS.Net Server");
                _writer.WriteLine($"==============");
                _writer.WriteLine($"Starting @");
                _writer.WriteLine($"    Local time: {DateTime.Now:yyyy/MMM/dd - hh:mm:ss tt}");
                _writer.WriteLine($"    UTC time:   {DateTime.UtcNow:yyyy/MMM/dd - hh:mm:ss tt}");
                _writer.WriteLine($"-------------------------------------------------------");

                _writer.Flush();
                _writer.AutoFlush = true;
            }
            catch (Exception ex)
            {
                _writer = null;
                throw new Exception($"Cannot write logfile \"{filename}\" - {ex.Message}");
            }

            _logLevel = logLevel;
            _timestamp = timestamp;
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.WriteLine($"-------------------------------------------------------");
                _writer.WriteLine($"Stopping @");
                _writer.WriteLine($"    Local time: {DateTime.Now:yyyy/MMM/dd - hh:mm:ss tt}");
                _writer.WriteLine($"    UTC time:   {DateTime.UtcNow:yyyy/MMM/dd - hh:mm:ss tt}");
                _writer.WriteLine($"-------------------------------------------------------");
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
                if (_timestamp)
                {
                    _writer?.WriteLine($"{DateTime.Now:hh:mm:ss.fff} Error: {message}");
                }
                else
                {
                    _writer?.WriteLine($"Error: {message}");
                }
            }
        }

        public void Info(string message)
        {
            if (_logLevel == LogLevel.Information)
            {
                lock (this)
                {
                    if (_timestamp)
                    {
                        _writer?.WriteLine($"{DateTime.Now:hh:mm:ss.fff} {message}");
                    }
                    else
                    {
                        _writer?.WriteLine(message);
                    }
                }
            }
        }

        public void Warning(string message)
        {
            if (_logLevel != LogLevel.Error)
            {
                lock (this)
                {
                    if (_timestamp)
                    {
                        _writer?.WriteLine($"{DateTime.Now:hh:mm:ss.fff} Warning: {message}");
                    }
                    else
                    {
                        _writer?.WriteLine($"Warning: {message}");
                    }
                }
            }
        }
    }
}
