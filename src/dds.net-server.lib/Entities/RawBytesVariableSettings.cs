namespace DDS.Net.Server.Entities
{
    public class RawBytesVariableSettings : BaseVariableSettings
    {
        public byte[] Data { get; private set; }

        public RawBytesVariableSettings(string name, byte[] data) : base(name)
        {
            Data = data;
        }
    }
}
