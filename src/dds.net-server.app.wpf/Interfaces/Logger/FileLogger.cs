using DDS.Net.Server.Interfaces;
using System;
using System.IO;

namespace DDS.Net.Server.WpfApp.Interfaces.Logger
{
    internal class FileLogger : ILogger, IDisposable
    {
        private readonly LogLevel _logLevel;
        private StreamWriter? _writer;

        public FileLogger(string filename, LogLevel logLevel = LogLevel.Information)
        {
            try
            {
                _writer = File.CreateText(filename);
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
                _writer.Dispose();
            }
        }

        public void Error(string message)
        {
            _writer?.WriteLine($"Error: {message}");
        }

        public void Info(string message)
        {
            if (_logLevel == LogLevel.Information)
            {
                _writer?.WriteLine(message);
            }
        }

        public void Warning(string message)
        {
            if (_logLevel != LogLevel.Error)
            {
                _writer?.WriteLine($"Warning: {message}");
            }
        }
    }
}
