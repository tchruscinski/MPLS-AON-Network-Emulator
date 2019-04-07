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
    class ManagementSystem
    {
        private UDPSocket listeningSocket = new UDPSocket();
        private UDPSocket sendingSocket = new UDPSocket();
        private Parser parser = new Parser();
        private List<string> routersListeningPorts = new List<string>();
        private List<string> routersSendingPorts = new List<string>();
        private int portNumber = 100;
        private int connectionCloudListeningPort = 100;
        private string localIP;
        private string listeningPorts;
        private string sendingPorts;

        /**
        * metoda parsująca plik konfiguracyjny dla sieci
        * @ no arguments, void
        */
        private void ReadConfig()
        {
            routersListeningPorts = parser.ParseConfig("Router", "ListeningPort");
            routersSendingPorts = parser.ParseConfig("Router", "SendingPort");
            for(int i = 0; i < routersListeningPorts.Count; i++)
            {
                Console.WriteLine("nasłuchujący port routera: "+routersListeningPorts[i]);
                Console.WriteLine("wysyłający port routera: "+routersSendingPorts[i]);
            }
        }

        /**
        * Metoda ustawiająca lokalny adres IP maszyny
        * @ no arguments, void
        */
        public void SetLocalIPAddress()
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
                }
            }
        }

        /**
        * metoda startująca serwer na listeningSockecie systemu zarządzania
        * @ no arguments, void
        */
        private void StartServer()
        {
            SetLocalIPAddress();
            listeningSocket.RunServer(localIP, 100);
        }
        
        /* Metoda tworząca stringi z konfiguracją, które będą wysłane do chmury połączeń
        * @ no arguments, void
        */
        private void prepareStringsToSend()
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
        private void SendRoutingTable()
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
        private void ShowInterface()
        {
            Console.WriteLine("System Zarządzania v1.0");
            StartServer();
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
            ManagementSystem ms = new ManagementSystem();
            ms.ReadConfig();
            Console.ReadKey();
            ms.ShowInterface();
            Console.ReadKey();
        }
    }
}
