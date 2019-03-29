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
        private static Socket socket; //socket ktory wysyla pakiety do chmury
        private static IPEndPoint endPoint; //reprezentuje punkt koncowy sieci jako adres ip i numer portu
        private static IPAddress destinationIP; //adres ip do ktorego beda skierowane pakiety

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
        private static void ConfigureHost(int portNumber, string targetIP)
        {
            destinationIP = IPAddress.Parse(targetIP);
            endPoint = new IPEndPoint(destinationIP, portNumber);
        }

        /**
         * metoda wysyłająca wiadomość
         * @ message - treść wiadomości
        */
        private static void SendMessage(string message)
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

        /**
         * metoda parsująca argumenty metody Main Hosta, configurująca Hosta i wysyłająca wiadomość
         * @ args - tablica typu string
        */
        private static void ParseAndExecuteForHostArguments(string[] args)
        {
            int portNumber = Int32.Parse(args[0]);
            string targetIP = args[1];
            string message = args[2];

            Console.WriteLine(portNumber + " " + targetIP + " " + message);
            Console.ReadKey();

            //ConfigureHost(portNumber, targetIP);
            //SendMessage(message);
        }

        /**
         * metoda Main Hosta
         * @ args - tablica typu string
        */
        static void Main(string[] args)
        {
            ParseAndExecuteForHostArguments(args);
        }
    }
}