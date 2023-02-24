using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.WpfApp.Extensions
{
    internal static class NetworkExtensions
    {
        public static bool IsIPAddressAssignedToAnUpInterface(this string ipAddress)
        {
            NetworkInterface[] ifaces = NetworkInterface.GetAllNetworkInterfaces();

            bool isAssigned = false;

            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (NetworkInterface iface in ifaces)
                {
                    if (iface.OperationalStatus == OperationalStatus.Up)
                    {
                        foreach (
                            UnicastIPAddressInformation addressInfo
                            in
                            iface.GetIPProperties().UnicastAddresses)
                        {
                            if (addressInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                if (addressInfo.Address.ToString() == ipAddress)
                                {
                                    isAssigned = true;
                                    break;
                                }
                            }
                        }

                        if (isAssigned)
                        {
                            break;
                        }
                    }
                }
            }

            return isAssigned;
        }
    }
}
