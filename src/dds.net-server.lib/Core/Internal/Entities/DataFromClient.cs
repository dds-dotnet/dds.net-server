namespace DDS.Net.Server.Core.Internal.Entities
{
    internal class DataFromClient
    {
        public string ClientRef { get; }
        public byte[] Data { get; set; }

        public DataFromClient(string client, byte[] data)
        {
            ClientRef = client;
            Data = data;
        }
    }
}
