using DDS.Net.Server.Core.Internal;
using DDS.Net.Server.Entities;
using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server
{
    public enum ServerStatus
    {
        Stopped,
        Starting,
        Started,
        Stopping
    }

    public partial class DdsServer
    {
        private readonly ServerConfiguration _config;
        private readonly ILogger _logger;

        private ServerStatus _status = ServerStatus.Stopped;
        private BaseServer? _tcpServer;
        private BaseServer? _udpServer;

        public DdsServer(ServerConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _config = config;
            _tcpServer = null;
            _udpServer = null;

            if (_config.Logger != null)
                _logger = _config.Logger;
            else
                throw new Exception($"No instance of {nameof(ILogger)} is provided");
        }

        public void Start()
        {
            if (_status == ServerStatus.Stopped)
            {
                SetServerStatus(ServerStatus.Starting);

                PrintLogStarting();

                if (_tcpServer == null && _config.EnableTCP)
                {
                    try
                    {
                        _tcpServer = new TcpServer(
                            _config.ListeningAddressIPv4,
                            _config.ListeningPortTCP,
                            _config.MaxClientsTCP,
                            _logger);

                        _tcpServer.StartServer();
                    }
                    catch (Exception ex)
                    {
                        _tcpServer = null;
                        _logger.Error($"Cannot start TCP Server: {ex.Message}");
                    }
                }

                if (_udpServer == null && _config.EnableUDP)
                {
                    try
                    {
                        _udpServer = new UdpServer(
                            _config.ListeningAddressIPv4,
                            _config.ListeningPortUDP,
                            _config.MaxClientsUDP,
                            _logger);

                        _udpServer.StartServer();
                    }
                    catch (Exception ex)
                    {
                        _udpServer = null;
                        _logger.Error($"Cannot start UDP Server: {ex.Message}");
                    }
                }

                if (_tcpServer != null || _udpServer != null)
                {
                    SetServerStatus(ServerStatus.Started);
                }
                else
                {
                    SetServerStatus(ServerStatus.Stopped);
                }
            }
            else
            {
                _logger.Warning("Cannot start server when it is not fully stopped");
            }
        }

        public void Stop()
        {
            if (_status == ServerStatus.Started)
            {
                SetServerStatus(ServerStatus.Stopping);

                PrintLogStopping();

                if (_tcpServer != null)
                {
                    try
                    {
                        _tcpServer.StopServer();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"TCP Server threw error on stopping: {ex.Message}");
                    }

                    _tcpServer = null;
                }

                if (_udpServer != null)
                {
                    try
                    {
                        _udpServer.StopServer();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"UDP Server threw error on stopping: {ex.Message}");
                    }

                    _udpServer = null;
                }

                SetServerStatus(ServerStatus.Stopped);
            }
            else
            {
                _logger.Warning("Cannot stop server when it is not fully started");
            }
        }

        private void SetServerStatus(ServerStatus newStatus)
        {
            if (_status != newStatus)
            {
                _status = newStatus;
                ServerStatusChanged?.Invoke(this, _status);
            }
        }
    }
}
