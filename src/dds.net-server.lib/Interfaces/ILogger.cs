namespace DDS.Net.Server.Interfaces
{
    public enum LogLevel
    {
        Information, Warning, Error
    }

    public interface ILogger
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
