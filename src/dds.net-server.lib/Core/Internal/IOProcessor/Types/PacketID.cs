namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal enum PacketID
    {
        HandShake = 0,
        VariableRegistration = 1,
        PrimitivesUpdateAtServer = 2,
        PrimitivesUpdateFromServer = 3,

        UNKNOWN
    }
}
