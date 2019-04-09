using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Management_System
{
    /**
     * Klasa implementująca system zarządzający siecią MPLS
     * @ ManagementSystem
    */
    public static class ManagementSystem
    {
        private static UDPSocket listeningSocket = new UDPSocket();
        private static UDPSocket sendingSocket = new UDPSocket();
        private static Parser parser = new Parser();
        private static int portNumber = 100;
        private static int connectionCloudListeningPort = 100;
        private static string localIP;

        /**
        * Metoda parsująca plik konfiguracyjny dla routerow
        * @ routerName - string, nazwa docelowego routera
        */
        private static string ReadRouterConfig(string routerName)
        {
            string routerConfig = parser.ParseRouterTable("routers_config.xml", routerName);

            Console.WriteLine("sparsowany xml: "+ routerConfig);
            Console.ReadKey();

            return routerConfig;
        }

        /**
        * Metoda parsująca plik konfiguracyjny dla hostow
        * @ hostName - string, nazwa docelowego hosta
        */
        private static string ReadHostConfig(string hostName)
        {
            string hostConfig = parser.ParseHostTable("host_config.xml", hostName);

            Console.WriteLine("sparsowany xml: "+ hostConfig);
            Console.ReadKey();

            return hostConfig;
        }

        /**
        * Metoda ustawiająca lokalny adres IP maszyny
        * @ no arguments, void
        */
        public static void SetLocalIPAddress()
        {
            if(!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) 
            {
                localIP = null;
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    Console.Write(localIP);
                }
            }
        }

        /**
        * metoda startująca serwer na listeningSockecie systemu zarządzania
        * @ no arguments, void
        */
        private static void StartServer()
        {
            SetLocalIPAddress();
            listeningSocket.RunServer(localIP, 100);
        }

        /**
        * metoda wysyłająca tablice routingową do routera
        * @ routerName - string, nazwa docelowego routera
        */
        private static void SendRouterTable(string routerName)
        {   
            Console.WriteLine("Parosowanie konfiguracji dla " + routerName + " ...");
            string routerTable = ReadRouterConfig(routerName);
            Console.WriteLine("Wysyłanie konfiguracji do " + routerName + " ...");
            //sendingSocket.Connect(localIP, routerListeningPort);
            //sendingSocket.Send(routerTable);
            Console.WriteLine("Wysyłano pomyślnie.");
        }

        /**
        * metoda wysyłająca tablice routingową do hosta
        * @ hostName - string, nazwa docelowego hosta
        */
        private static void SendHostTable(string hostName)
        {   
            Console.WriteLine("Parosowanie konfiguracji dla " + hostName + " ...");
            string hostTable = ReadHostConfig(hostName);
            Console.WriteLine("Wysyłanie konfiguracji do " + hostName + " ...");
            //sendingSocket.Connect(localIP, hostListeningPort);
            sendingSocket.Send(hostTable);
            Console.WriteLine("Wysyłano pomyślnie.");
        }

        /**
        * metoda wyświetlająca interfejs systemu zarządznia
        * @ no arguments, void
        */
        private static void ShowInterface()
        {
            Console.WriteLine("System Zarządzania v1.0");
            StartServer();
            ReadHostConfig("Host");
            /*while ((line = Console.ReadLine()) != null)
            {
                ReadInput();
            }*/
        }

        /**
         * Metoda procesująca requesty przychodzące do systemu zarządzania
         * @ message - string, wiadomość z przychodzącego pakietu
        */
        public static void ProcessRequest(string message)
        {
            if(message == null)
            {
                return;
            }

            if(message.Contains("gettable"))
            {
                if(message.Contains("host"))
                {
                    //obsluga requesta po tabele od hosta
                }

                if(message.Contains("router"))
                {
                    //obsluga requesta po tabele od routera
                }
            }   
        }

         /**
         * Metoda czytająca komendy z konsoli
         * @ no arguments, void
        */
        /*private void ReadInput() 
        {
            string line = Console.ReadLine();
            //if(line = "")
        }*/

        /**
         * metoda Systemu Zarządzania
         * @ args - tablica typu string
        */
        static void Main(string[] args)
        {
            //ConfigureHosts();
            UDPSocket udpSocket = new UDPSocket();
            ManagementSystem.ShowInterface();
            Console.ReadKey();
        }
    }
}
