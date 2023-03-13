using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.IOProviders.SimpleServer.Types;
using DDS.Net.Server.Core.Internal.SimpleServer;
using DDS.Net.Server.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace DDS.Net.Server.Core.Internal.IOProviders.SimpleServer
{
    internal class SSTCP : SSBase
    {
        private readonly int SLEEP_TIME_MS_WHEN_NO_CONNECTION_WAITING = 100;
        private readonly int SLEEP_TIME_MS_WHEN_DONE_NOTHING = 10;

        private volatile bool isConnectionListenerThreadRunning = false;
        private volatile bool isDataReceiverThreadRunning = false;
        private Thread connectionListenerThread = null!;
        private Thread dataReceiverThread = null!;

        private List<Socket> connectedClients;

        public SSTCP(
            ISyncQueueReaderEnd<SSPacket> dataInputQueue,
            ISyncQueueWriterEnd<SSPacket> dataOutputQueue,

            string IPv4, ushort port, int maxClients, ILogger logger)

            : base(dataInputQueue, dataOutputQueue,
                   IPv4, port, maxClients, SSType.TCP, logger)
        {
            connectedClients = new();
        }

        public override void StartServer()
        {
            lock (this)
            {
                if (connectionListenerThread == null &&
                    localSocket != null)
                {
                    isConnectionListenerThreadRunning = true;
                    connectionListenerThread = new Thread(ConnectionListenerThread);
                    connectionListenerThread.IsBackground = true;
                    connectionListenerThread.Start();
                }
            }
        }

        public override void StopServer()
        {
            lock (this)
            {
                if (connectionListenerThread != null)
                {
                    isConnectionListenerThreadRunning = false;
                    connectionListenerThread.Join();
                }
            }
        }

        private void ConnectionListenerThread()
        {
            bool bindingOk = false;

            if (localSocket != null)
            {
                try
                {
                    localSocket.Bind(localEndPoint);
                    localSocket.Listen(10);

                    logger.Info($"TCP socket bound @{localEndPoint}");

                    bindingOk = true;
                }
                catch (Exception ex)
                {
                    logger.Error($"TCP socket binding error @{localEndPoint}: {ex.Message}");
                }
            }

            if (bindingOk && localSocket != null)
            {
                SetServerStatus(SSStatus.Running);

                if (isConnectionListenerThreadRunning)
                {
                    logger.Info($"SSTCP server running @{localEndPoint}");
                }

                isDataReceiverThreadRunning = true;
                dataReceiverThread = new Thread(DataReceiverThread);
                dataReceiverThread.IsBackground = true;
                dataReceiverThread.Start();

                while (isConnectionListenerThreadRunning)
                {
                    try
                    {
                        Socket newSocket = localSocket.Accept();

                        lock (this)
                        {
                            if (connectedClients.Count < maxNumberOfClients)
                            {
                                connectedClients.Add(newSocket);

                                logger.Info($"SSTCP new connection accepted from {newSocket.RemoteEndPoint}");
                            }
                            else
                            {
                                logger.Warning($"SSTCP rejecting new connection from {newSocket.RemoteEndPoint}, as number of connected clients is {connectedClients.Count}");

                                newSocket.Close();
                            }
                        }
                    }
                    catch (SocketException)
                    {
                        // Doing nothing as we are in non-blocking mode; so,
                        //     Accept is expected to throw SocketException

                        Thread.Sleep(SLEEP_TIME_MS_WHEN_NO_CONNECTION_WAITING);
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"SSTCP socket accepting failed: {ex.Message}");
                    }
                }

                logger.Info($"SSTCP server @{localEndPoint} exiting - waiting for data receiver to exit");

                isDataReceiverThreadRunning = false;

                try
                {
                    dataReceiverThread.Join(1000);
                }
                catch (Exception) { }

                logger.Info($"SSTCP server @{localEndPoint} exited");
            }

            isConnectionListenerThreadRunning = false;
            connectionListenerThread = null!;

            SetServerStatus(SSStatus.Stopped);
        }

        private void DataReceiverThread()
        {
            while (isDataReceiverThreadRunning)
            {
                bool hasDoneAnythingInIteration = false;

                //- 
                //- Receiving data from clients when available
                //- 
                lock (this)
                {
                    foreach (Socket socket in connectedClients)
                    {
                        if (socket.Connected == false)
                        {
                            connectedClients.Remove(socket);

                            //- sending null to packet processor to indicate that the client has disconnected
                            //
                            dataOutputQueue.Enqueue(new SSPacket((IPEndPoint)socket.RemoteEndPoint!, null!));

                            logger.Info($"SSTCP connection from {socket.RemoteEndPoint} lost");

                            hasDoneAnythingInIteration = true;

                            break;
                        }

                        int dataAvailable = socket.Available;

                        if (dataAvailable > 0)
                        {
                            byte[] bytes = new byte[dataAvailable];
                            socket.Receive(bytes);

                            dataOutputQueue.Enqueue(new SSPacket((IPEndPoint)socket.RemoteEndPoint!, bytes));

                            hasDoneAnythingInIteration = true;
                        }
                    }
                }


                //- 
                //- Sending data to clients
                //- 
                while (dataInputQueue.CanDequeue())
                {
                    hasDoneAnythingInIteration = true;

                    SSPacket packet = dataInputQueue.Dequeue();

                    lock (this)
                    {
                        foreach (Socket socket in connectedClients)
                        {
                            IPEndPoint sockEP = (IPEndPoint)socket.RemoteEndPoint!;

                            if (sockEP.Address.Equals(packet.ClientInfo.Address) &&
                                sockEP.Port == packet.ClientInfo.Port)
                            {
                                if (socket.Connected)
                                {
                                    socket.Send(packet.PacketData);
                                }
                                else
                                {
                                    connectedClients.Remove(socket);

                                    //- sending null to packet processor to indicate that the client has disconnected
                                    //
                                    dataOutputQueue.Enqueue(new SSPacket((IPEndPoint)socket.RemoteEndPoint!, null!));

                                    logger.Warning($"SSTCP connection from {socket.RemoteEndPoint} lost - cannot send data");
                                }

                                break;
                            }
                        }
                    }
                }

                if (hasDoneAnythingInIteration == false)
                {
                    Thread.Sleep(SLEEP_TIME_MS_WHEN_DONE_NOTHING);
                }
            }
        }
    }
}
