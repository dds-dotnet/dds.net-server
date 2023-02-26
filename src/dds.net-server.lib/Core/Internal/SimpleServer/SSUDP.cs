﻿using DDS.Net.Server.Core.Internal.SimpleServer.Types;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal class SSUDP : SSBase
    {
        private volatile bool isClientListenerThreadRunning = false;
        private Thread? clientListenerThread = null;

        public SSUDP(string IPv4, ushort port, int maxClients, ILogger logger)
            : base(IPv4, port, maxClients, SSType.UDP, logger)
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
                    clientListenerThread.Join();
                }
            }
        }

        public override SSPacketSendingStatus SendPacket(SSPacket packet)
        {
            throw new NotImplementedException();
        }

        private void ClientListenerThread()
        {
            bool bindingOk = false;

            try
            {
                localSocket?.Bind(localEndPoint);

                logger.Info($"UDP socket bound @{localEndPoint}");

                bindingOk = true;
            }
            catch(Exception ex)
            {
                logger.Error($"UDP socket binding error @{localEndPoint}: {ex.Message}");
            }

            if (bindingOk)
            {
                SetServerStatus(SSStatus.Running);

                while (isClientListenerThreadRunning)
                {

                }
            }

            isClientListenerThreadRunning = false;
            clientListenerThread = null;

            SetServerStatus(SSStatus.Stopped);
        }
    }
}