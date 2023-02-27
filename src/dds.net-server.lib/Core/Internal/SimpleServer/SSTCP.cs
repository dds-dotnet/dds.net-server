using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.SimpleServer.Types;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal class SSTCP : SSBase
    {
        private volatile bool isConnectionListenerThreadRunning = false;
        private volatile bool isDataReceiverThreadRunning = false;
        private Thread connectionListenerThread = null!;
        private Thread dataReceiverThread = null!;

        private List<Socket> connectedClients;

        public SSTCP(
            ISyncDataInputQueueEnd<SSPacket> dataInputQueue,
            ISyncDataOutputQueueEnd<SSPacket> dataOutputQueue,
            
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

        public override SSPacketSendingStatus SendPacket(SSPacket packet)
        {
            throw new NotImplementedException();
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
                    catch(SocketException)
                    {
                        // Doing nothing as we are in non-blocking mode; so,
                        //     Accept is expected to throw SocketException
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"SSTCP socket accepting failed: {ex.Message}");
                    }
                }

                logger.Info($"SSTCP server @{localEndPoint} exiting - waiting for data receiver to exit");

                isDataReceiverThreadRunning = false;
                dataReceiverThread.Join();

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
                lock (this)
                {
                    foreach (Socket socket in connectedClients)
                    {
                        if (socket.Connected == false)
                        {
                            connectedClients.Remove(socket);

                            logger.Info($"SSTCP connection from {socket.RemoteEndPoint} lost");

                            break;
                        }

                        int dataAvailable = socket.Available;

                        if (dataAvailable > 0)
                        {
                            byte[] bytes = new byte[dataAvailable];
                            socket.Receive(bytes);

                            dataOutputQueue.Enqueue(new SSPacket((IPEndPoint)socket.RemoteEndPoint!, bytes));
                        }
                    }
                }

                while (dataInputQueue.CanDequeue())
                {
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

                                    logger.Warning($"SSTCP connection from {socket.RemoteEndPoint} lost - cannot send data");
                                }

                                break;
                            }
                        }
                    }
                }

                Thread.Yield();
            }
        }
    }
}
