namespace DDS.Net.Server.Core.Internal
{
    internal static class QueueSizeSettings
    {
        public static readonly int NETWORK_DATA_FROM_CLIENTS_QUEUE_SIZE = 1000;
        public static readonly int NETWORK_DATA_TO_CLIENTS_QUEUE_SIZE = 1000;
        public static readonly int NETWORK_COMMANDS_QUEUE_SIZE = 20;
        public static readonly int NETWORK_RESPONSES_QUEUE_SIZE = 20;

        public static readonly int VARS_HANDLER_COMMANDS_QUEUE_SIZE = 20;
        public static readonly int VARS_HANDLER_RESPONSES_QUEUE_SIZE = 20;

        public static readonly int TCP_DATA_FROM_CLIENTS_QUEUE_SIZE = 1000;
        public static readonly int TCP_DATA_TO_CLIENTS_QUEUE_SIZE = 1000;

        public static readonly int UDP_DATA_FROM_CLIENTS_QUEUE_SIZE = 1000;
        public static readonly int UDP_DATA_TO_CLIENTS_QUEUE_SIZE = 1000;
    }
}
