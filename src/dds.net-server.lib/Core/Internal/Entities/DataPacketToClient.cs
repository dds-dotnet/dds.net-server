namespace DDS.Net.Server.Core.Internal.Entities
{
    internal class DataPacketToClient
    {
        public string ClientRef { get; }
        public byte[] Data { get; set; }

        public DataPacketToClient(string client, byte[] data)
        {
            ClientRef = client;
            Data = data;
        }
    }
}
