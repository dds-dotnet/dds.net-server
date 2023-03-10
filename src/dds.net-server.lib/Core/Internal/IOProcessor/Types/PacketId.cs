namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal enum PacketId
    {
        /// <summary>
        /// Initialization information / configuration exchange
        /// </summary>
        HandShake = 0,
        /// <summary>
        /// Registering variables with the server
        /// </summary>
        VariablesRegistration = 1,
        /// <summary>
        /// Updating variable values at the server
        /// </summary>
        VariablesUpdateAtServer = 2,
        /// <summary>
        /// Updating variable values at the client
        /// </summary>
        VariablesUpdateFromServer = 3,
        /// <summary>
        /// Error responses from the server
        /// </summary>
        ErrorResponseFromServer = 4,




        /// <summary>
        /// Unknown packet
        /// </summary>
        UnknownPacket
    }
}
