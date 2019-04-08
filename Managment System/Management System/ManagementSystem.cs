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
        private static List<string> routersListeningPorts = new List<string>();
        private static List<string> routersSendingPorts = new List<string>();
        private static int portNumber = 100;
        private static int connectionCloudListeningPort = 100;
        private static string localIP;
        private static string listeningPorts;
        private static string sendingPorts;

        /**
        * metoda parsująca plik konfiguracyjny dla routerow
        * @ no arguments, void
        */
        private static void ReadRouterConfig()
        {
            string routerConfig = parser.ParseRouterTable("routers_config.xml", "Router1");

            Console.WriteLine("sparsowany xml: "+ routerConfig);
            Console.ReadKey();
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
        
        /* Metoda tworząca stringi z konfiguracją, które będą wysłane do chmury połączeń
        * @ no arguments, void
        */
        private static void prepareStringsToSend()
        {
            if(routersListeningPorts.Count == 0 || routersSendingPorts.Count == 0) 
            {
                return;
            }

            for(int i = 0; i < routersListeningPorts.Count; i++)
            {   
                listeningPorts += routersListeningPorts[i] + ",";
                sendingPorts += routersSendingPorts[i] + ",";
            }
        }

        /**
        * metoda wysyłająca tablice routingową do chmury połączeń
        * @ no arguments, void
        */
        private static void SendRoutingTable()
        {   
            Console.WriteLine("Wysyłanie konfiguracji do chmury połączeń ...");
            prepareStringsToSend();
            sendingSocket.Connect(localIP, connectionCloudListeningPort);
            sendingSocket.Send(listeningPorts);
            sendingSocket.Send(sendingPorts);
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
            ReadRouterConfig();
            SendRoutingTable();
            
            /*while ((line = Console.ReadLine()) != null)
            {
                ReadInput();
            }*/
        }

        /**
         * Metoda czytająca komendy z konsoli
         * @ no arguments, void
        */
        public static void ProcessRequest(string message)
        {
            if(message == null)
            {
                return;
            }

            if(message == "gettable")
            {
                //sendingSocket.Send(routertable);
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
         * metoda konfigurująca Hosta, odpala apke Hosta z określonymi parametrami
         * w przyszłości konfiguracja będzie czytana z pliku
         * @ no arguments, void
        */
        private static void ConfigureHosts()
        {
            try
            {
                Process startHost = new Process();
                //chwilowo przykładowa ścieżka do procesu
                startHost.StartInfo.FileName = "C://Users/tomas/Desktop/TSST Projekt/tsst-network-emulator/Host/Host/Host.exe";

                // portNumber targetIP message
                startHost.StartInfo.Arguments = "100 10.10.10 \"message\"";
                startHost.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                startHost.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

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
