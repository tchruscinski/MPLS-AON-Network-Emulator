﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPTestSend
{   
    class Program
    {   
        public static string GetLocalIPAddress()
        {   
            if(!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        static void Main(string[] args)
        {
            //address family okresla schemat adresowania
            //internetwork = adres ipv4
            //dgram czyli socket datagramowy czyli udp
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            String localIp = GetLocalIPAddress();
            IPAddress broadcast = IPAddress.Parse(localIp);
            
            String text = "testtestest";
            byte[] sendbuf = Encoding.ASCII.GetBytes(text);
            IPEndPoint ep = new IPEndPoint(broadcast, 11000);

            s.SendTo(sendbuf, ep);
            Console.WriteLine("Message sent to the broadcast address");
            Console.ReadKey();
        }
    }
}
