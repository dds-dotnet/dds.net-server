namespace DDS.Net.Server.Core.Internal.Base.Entities
{ 
    internal enum DataIOProviderRunningStatus
    {
        Stopped,
        Starting,
        Started,
        Paused,
        Stopping
    }

    internal class DataIOProviderStatus
    {
        public DataIOProviderRunningStatus Status { get; set; }
    }
}
