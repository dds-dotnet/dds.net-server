# ServerConfiguration

DDS.Net.Server.Entities.**ServerConfiguration** provides settings for server initialization. The settings include:

  * string *ListeningAddressIPv4* - the listening IPv4 address for receiving connections and data.
  * bool *EnableTCP* - indicates to the server whether the TCP connectivity be enabled or not.
  * ushort *ListeningPortTCP* - the local TCP port for receiving connections.
  * int *MaxClientsTCP* - maximum number of TCP clients.
  * bool *EnableUDP* - indicates to the server whether the UDP connectivity be enabled or not.
  * ushort *ListeningPortUDP* - the local UDP port for receiving and transmitting the data.
  * ILogger *Logger* - the implementation of [ILogger](./ILogger.md) interface that should be used by the server.






