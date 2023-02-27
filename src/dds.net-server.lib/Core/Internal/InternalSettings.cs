namespace DDS.Net.Server.Core.Internal
{
    internal static class InternalSettings
    {
        public static readonly int NETWORK_DATA_FROM_CLIENTS_QUEUE_SIZE = 1000;
        public static readonly int NETWORK_DATA_TO_CLIENTS_QUEUE_SIZE = 1000;

        public static readonly int TCP_CLIENTS_INPUT_QUEUE_SIZE = 1000;
        public static readonly int TCP_CLIENTS_OUTPUT_QUEUE_SIZE = 1000;

        public static readonly int UDP_CLIENTS_INPUT_QUEUE_SIZE = 1000;
        public static readonly int UDP_CLIENTS_OUTPUT_QUEUE_SIZE = 1000;
    }
}
