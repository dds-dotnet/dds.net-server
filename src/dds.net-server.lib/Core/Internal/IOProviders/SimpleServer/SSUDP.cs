using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.IOProviders.SimpleServer.Types;
using DDS.Net.Server.Core.Internal.SimpleServer;
using DDS.Net.Server.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace DDS.Net.Server.Core.Internal.IOProviders.SimpleServer
{
    internal class SSUDP : SSBase
    {
        private readonly int SLEEP_TIME_MS_WHEN_DONE_NOTHING = 10;

        private volatile bool isClientListenerThreadRunning = false;
        private Thread? clientListenerThread = null;

        public SSUDP(
            ISyncQueueReaderEnd<SSPacket> dataInputQueue,
            ISyncQueueWriterEnd<SSPacket> dataOutputQueue,

            string IPv4, ushort port, ILogger logger)

            : base(dataInputQueue, dataOutputQueue,
                   IPv4, port, int.MaxValue, SSType.UDP, logger)
        {
        }

        public override void StartServer()
        {
            lock (this)
            {
                if (clientListenerThread == null &&
                    localSocket != null)
                {
                    isClientListenerThreadRunning = true;
                    clientListenerThread = new Thread(ClientListenerThread);
                    clientListenerThread.IsBackground = true;
                    clientListenerThread.Start();
                }
            }
        }

        public override void StopServer()
        {
            lock (this)
            {
                if (clientListenerThread != null)
                {
                    isClientListenerThreadRunning = false;

                    try
                    {
                        clientListenerThread.Join(1000);
                    }
                    catch { }
                }
            }
        }

        private void ClientListenerThread()
        {
            bool bindingOk = false;

            if (localSocket != null)
            {
                try
                {
                    localSocket.Bind(localEndPoint);

                    logger.Info($"UDP socket bound @{localEndPoint}");

                    bindingOk = true;
                }
                catch (Exception ex)
                {
                    logger.Error($"UDP socket binding error @{localEndPoint}: {ex.Message}");
                }
            }

            if (bindingOk && localSocket != null)
            {
                SetServerStatus(SSStatus.Running);

                if (isClientListenerThreadRunning)
                {
                    logger.Info($"SSUDP server running @{localEndPoint}");
                }

                while (isClientListenerThreadRunning)
                {
                    bool hasDoneAnythingInIteration = false;

                    //- 
                    //- Reading from socket and enqueuing received data packet
                    //- 

                    while (true)
                    {
                        if (dataOutputQueue.CanEnqueue() == false)
                        {
                            break;
                        }

                        try
                        {
                            int dataAvailable = localSocket.Available;

                            if (dataAvailable > 0)
                            {
                                byte[] data = new byte[dataAvailable];

                                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                                EndPoint senderRemote = sender;

                                localSocket.ReceiveFrom(data, SocketFlags.None, ref senderRemote);

                                dataOutputQueue.Enqueue(new SSPacket((IPEndPoint)senderRemote, data));

                                hasDoneAnythingInIteration = true;
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch(Exception ex)
                        {
                            logger.Error($"SSUDP data reception failed - {ex.Message}");
                            break;
                        }
                    }


                    //- 
                    //- Reading data from input queue and sending out through socket
                    //- 

                    while (dataInputQueue.CanDequeue())
                    {
                        SSPacket outData = dataInputQueue.Dequeue();

                        try
                        {
                            localSocket.SendTo(outData.PacketData, outData.ClientInfo);
                        }
                        catch (Exception ex)
                        {
                            logger.Error($"SSUDP data sending failed - {ex.Message}");
                        }

                        hasDoneAnythingInIteration = true;
                    }


                    //- 
                    //- Sleep when nothing is done :-)
                    //- 

                    if (hasDoneAnythingInIteration == false)
                    {
                        Thread.Sleep(SLEEP_TIME_MS_WHEN_DONE_NOTHING);
                    }
                }

                logger.Info($"SSUDP server @{localEndPoint} exited");
            }

            isClientListenerThreadRunning = false;
            clientListenerThread = null;

            SetServerStatus(SSStatus.Stopped);
        }
    }
}
