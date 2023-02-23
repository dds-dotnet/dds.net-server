namespace DDS.Net.Server.Interfaces
{
    internal interface ILogger
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
