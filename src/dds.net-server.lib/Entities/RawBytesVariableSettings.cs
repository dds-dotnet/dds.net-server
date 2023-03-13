namespace DDS.Net.Server.Entities
{
    public class RawBytesVariableSettings : VariableSettings
    {
        public byte[] Data { get; private set; }

        public RawBytesVariableSettings(string name, byte[] data) : base(name)
        {
            Data = data;
        }
    }
}
