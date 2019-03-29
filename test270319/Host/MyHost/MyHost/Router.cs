using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPTestSend
{
    class Router
    {
        private Socket socket; //socket ktory wysyla pakiety do chmury
        private IPEndPoint endPoint; //reprezentuje punkt koncowy sieci jako adres ip i numer portu
        private IPAddress broadcast; //adres ip do ktorego beda skierowane pakiety
        public Router(int portNumber, string targetIp)
        {
            //address family okresla schemat adresowania
            //internetwork = adres ipv4
            //dgram czyli socket datagramowy czyli udp
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            broadcast = IPAddress.Parse(targetIp);
            endPoint = new IPEndPoint(broadcast, portNumber);
        }

        public void SendMessage()
        {
            String text = "testtestest";
            byte[] sendbuf = Encoding.ASCII.GetBytes(text);

            socket.SendTo(sendbuf, endPoint);
            Console.WriteLine("Message sent to the broadcast address");
        }
    }
}
