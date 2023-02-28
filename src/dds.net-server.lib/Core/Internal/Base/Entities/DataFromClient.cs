namespace DDS.Net.Server.Core.Internal.Base.Entities
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
