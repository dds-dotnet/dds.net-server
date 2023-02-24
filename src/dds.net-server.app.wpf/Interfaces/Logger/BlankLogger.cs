using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server.WpfApp.Interfaces.Logger
{
    internal class BlankLogger : ILogger
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
