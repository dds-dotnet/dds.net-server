namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal enum PacketID
    {
        /// <summary>
        /// Information exchange
        /// </summary>
        HandShake = 0,
        /// <summary>
        /// Registering variables with the server
        /// </summary>
        VariableRegistration = 1,
        /// <summary>
        /// Updating primitive variable values at the server
        /// </summary>
        PrimitivesUpdateAtServer = 2,
        /// <summary>
        /// Updating primitive variable values at the client
        /// </summary>
        PrimitivesUpdateFromServer = 3,




        /// <summary>
        /// Unknown packet
        /// </summary>
        UNKNOWN
    }
}
