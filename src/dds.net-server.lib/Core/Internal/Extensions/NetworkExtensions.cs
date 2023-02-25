using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace DDS.Net.Server.Core.Internal.Extensions
{
    internal static class NetworkExtensions
    {
        private static Regex ipv4AddressPattern = new Regex(@"\s*(\d{1,3})\s*\.\s*(\d{1,3})\s*\.\s*(\d{1,3})\s*\.\s*(\d{1,3})\s*");

        public static bool IsValidIPv4Address(this string ipv4Address)
        {
            return ipv4AddressPattern.IsMatch(ipv4Address);
        }

        public static bool IsInvalidIPv4Address(this string ipv4Address)
        {
            return ipv4AddressPattern.IsMatch(ipv4Address) == false;
        }

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
