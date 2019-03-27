using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Host
{
    //Klasa implementujacą hosta
    class Host
    {
        private Socket socket; //socket ktory wysyla pakiety do chmury
        private IPEndPoint endPoint; //reprezentuje punkt koncowy sieci jako adres ip i numer portu
        private IPAddress destinationIP; //adres ip do ktorego beda skierowane pakiety

        public Host()
        {
            //dgram czyli socket datagramowy czyli udp
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        /**
         * metoda konfigurująca parametry hosta:
         * @ portNumber port, na który będzie wysyłać pakiety 
         * @ targetIP IP docelowe
        */
        public void ConfigureHost(int portNumber, string targetIP)
        {
            destinationIP = IPAddress.Parse(targetIP);
            endPoint = new IPEndPoint(destinationIP, portNumber);
        }

        /**
         * metoda wysyłająca wiadomość
         * @ message - treść wiadomości
        */
        public void sendMessage(string message)
        {
            //String text = "testtestest";
            byte[] sendbuf = Encoding.ASCII.GetBytes(message);

            socket.SendTo(sendbuf, endPoint);
            Console.WriteLine("Message sent to the broadcast address");
        }

        /**
         * metoda zwracająca lokalny adres IP
         * @ no arguments, string return type
        */
        private static string GetLocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
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
    }
}