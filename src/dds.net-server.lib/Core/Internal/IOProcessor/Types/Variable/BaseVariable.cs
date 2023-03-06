namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal abstract class BaseVariable
    {
        public ushort Id { get; private set; }
        public string Name { get; private set; }

        public List<VariableProvider> Providers { get; set; } = new();

        public BaseVariable(ushort id, string name)
        {
            Id = id;
            Name = name;

        }
    }
}
