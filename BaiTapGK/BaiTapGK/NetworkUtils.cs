using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace BaiTapGK
{
    public static class NetworkUtils
    {
        /// <summary>
        /// Ki?m tra xem port có ???c s? d?ng hay không
        /// </summary>
        public static bool IsPortAvailable(int port)
        {
            try
            {
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

                foreach (IPEndPoint endpoint in tcpConnInfoArray)
                {
                    if (endpoint.Port == port)
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// L?y ??a ch? IP local
        /// </summary>
        public static string GetLocalIPAddress()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
                
                foreach (IPAddress ip in hostEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        /// <summary>
        /// Ki?m tra k?t n?i internet
        /// </summary>
        public static bool IsInternetAvailable()
        {
            try
            {
                using (var ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 3000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// T?o Room ID ng?u nhiên
        /// </summary>
        public static string GenerateRoomId()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        /// <summary>
        /// Validate Room ID format
        /// </summary>
        public static bool IsValidRoomId(string roomId)
        {
            return !string.IsNullOrWhiteSpace(roomId) && 
                   roomId.Length == 4 && 
                   int.TryParse(roomId, out _);
        }
    }
}