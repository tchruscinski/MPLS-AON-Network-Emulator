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
        private static string localIP = "127.0.0.1"; //docelowe ip;

        /**
        * Metoda parsująca plik konfiguracyjny dla routerow
        * @ routerName - string, nazwa docelowego routera
        */
        private static string ReadRouterConfig(string routerName)
        {
            string routerConfig = parser.ParseRouterTable("routers_config.xml", routerName);

            Console.WriteLine("sparsowany xml: "+ routerConfig);

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

            return hostConfig;
        }


        //ON HOLD, używamy static stringa jako IP
        /**
        * Metoda ustawiająca lokalny adres IP maszyny
        * @ no arguments, void
        */
        /*public static void SetLocalIPAddress()
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
        }*/

        /**
        * metoda startująca serwer na listeningSockecie systemu zarządzania
        * @ no arguments, void
        */
        private static void StartServer()
        {
            listeningSocket.RunServer(localIP, portNumber);
        }

        /**
        * metoda wysyłająca tablice routingową do routera
        * @ routerName - string, nazwa docelowego routera
        */
        private static void SendRouterTable(string routerName)
        {   
            Console.WriteLine("Parosowanie konfiguracji dla " + routerName + " ...");
            string routerTable = ReadRouterConfig(routerName);
            //sendingSocket.Connect(localIP, 1);
            sendingSocket.Send(routerTable);
            Console.WriteLine("Wysyłanie konfiguracji do " + routerName + " ...");
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("System Zarządzania v1.0");

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(" _   _ ___  ___ _____  ");
            Console.WriteLine("| \\ | ||  \\/  |/  ___|");
            Console.WriteLine("|  \\| || .  . |\\ `--.  ");
            Console.WriteLine("| . ` || |\\/| | `--. \\");
            Console.WriteLine("| |\\  || |  | |/\\__/ / ");
            Console.WriteLine("\\_| \\_/\\_|  |_/\\____/  ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Gray;
            StartServer();
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

            Console.WriteLine("otrzymano: " + message);

            if(message.Contains("Host"))
            {
                Console.WriteLine("Otrzymano request od " + message + " o tabele hosta");
                //sendingSocket.Connect(localIP, 1);
                SendHostTable(message);
                Console.WriteLine("Tabela wysłana do " + message);
            }

            if(message.Contains("Router"))
            {
                Console.WriteLine("Otrzymano request od " + message + " o tabele routera");
                SendRouterTable(message);
                Console.WriteLine("Tabela wysłana do " + message);
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

        private static void ShowHelp()
        {
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("---POMOC---");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" ");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("change-config [router_name]");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Zmiana konfiguracji routera na podstawie odpowiadajacego mu pliku XML");
        }


        private static void RunCommand(string input)
        {
            String[] comm;
            if(input == "help" || input == "h" || input == "?")
            {
                ShowHelp();

            }
            else
            {
                comm = input.Split(' ');

                if(comm[0] == "change-config")
                {
                    Console.WriteLine("TODO");

                }
                
            }
        }

        /**
         * metoda Systemu Zarządzania
         * @ args - tablica typu string
        */
        static void Main(string[] args)
        {
            string command;
        
            UDPSocket udpSocket = new UDPSocket();
            sendingSocket.Client("127.0.0.1", 1);
            //ConfigureHosts();
            //sendingSocket.Send("NMS;Chuj");
            //SendRouterTable("Router1");
            ManagementSystem.ShowInterface();
            while(true)
            {
                Console.Write("NMS# ");
                command = Console.ReadLine();
                RunCommand(command);



            }
            
        }
    }
}
