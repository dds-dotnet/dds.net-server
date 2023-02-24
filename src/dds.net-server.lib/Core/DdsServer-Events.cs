namespace DDS.Net.Server
{
    public partial class DdsServer
    {
        public event EventHandler<ServerStatus>? ServerStatusChanged;
    }
}
