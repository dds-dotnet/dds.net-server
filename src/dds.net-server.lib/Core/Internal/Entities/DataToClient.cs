namespace DDS.Net.Server.Core.Internal.Entities
{
    internal class DataToClient
    {
        public string ClientRef { get; }
        public byte[] Data { get; set; }

        public DataToClient(string client, byte[] data)
        {
            ClientRef = client;
            Data = data;
        }
    }
}
