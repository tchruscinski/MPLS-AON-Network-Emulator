using System;
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
        static void Main(string[] args)
        {
            //address family okresla schemat adresowania
            //internetwork = adres ipv4
            //dgram czyli socket datagramowy czyli udp
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress broadcast = IPAddress.Parse("192.168.1.255");
            
            String text = "testtestest";
            byte[] sendbuf = Encoding.ASCII.GetBytes(text);
            IPEndPoint ep = new IPEndPoint(broadcast, 11000);

            s.SendTo(sendbuf, ep);
            Console.WriteLine("Message sent to the broadcast address");
        }
    }
}
