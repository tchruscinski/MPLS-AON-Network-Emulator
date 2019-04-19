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
        //private static UDPSocket listeningSocket = new UDPSocket();
        //private static UDPSocket sendingSocket = new UDPSocket();
        private static Parser parser = new Parser();
        private static int portNumber = 100;
        private static int connectionCloudListeningPort = 100;
        private static string localIP = "127.0.0.1"; //docelowe ip;
        private static List<UDPSocket> routerSendingSockets = new List<UDPSocket>();
        private static List<UDPSocket> routerReceivingSockets = new List<UDPSocket>();
        private static List<UDPSocket> hostSendingSockets = new List<UDPSocket>();
        private static List<UDPSocket> hostReceivingSockets = new List<UDPSocket>();
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

            Console.WriteLine("sparsowany xml: " + hostConfig);

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
            //listeningSocket.RunServer(localIP, portNumber);
            for (int i = 0; i < routerReceivingSockets.Count; i++)
                routerReceivingSockets[i].RunServer(localIP, routerReceivingSockets[i].GetPort());
            for (int i = 0; i < hostReceivingSockets.Count; i++)
                hostReceivingSockets[i].RunServer(localIP, hostReceivingSockets[i].GetPort());
        }
        /**
       * metoda startująca clienty na receivingSocketach systemu zarządzania
       * @ no arguments, void
       */
        private static void StartClient()
        {
            //listeningSocket.RunServer(localIP, portNumber);
            for (int i = 0; i < routerSendingSockets.Count; i++)
                routerSendingSockets[i].Client(localIP, routerSendingSockets[i].GetPort());
            for (int i = 0; i < hostSendingSockets.Count; i++)
                hostSendingSockets[i].Client(localIP, hostSendingSockets[i].GetPort());
        }

        /**
        * metoda wysyłająca tablice routingową do routera
        * @ routerName - string, nazwa docelowego routera
        */
        private static void SendRouterTable(string routerName)
        {   
            Console.WriteLine("Parosowanie konfiguracji dla " + routerName + " ...");
            StringBuilder builder = new StringBuilder();
            builder.Append("NMS;");
            builder.Append(ReadRouterConfig(routerName));          
            string routerTable = builder.ToString();
            //sendingSocket.Client(localIP, 1);
            //sendingSocket.Send(routerTable);
            if(routerName.Equals("Router1"))
            {
                //routerSendingSockets[0].Client(localIP, 1);
                routerSendingSockets[0].Send(routerTable);
            }
            if (routerName.Equals("Router2"))
            {
                //routerSendingSockets[1].Client(localIP, 1);
                routerSendingSockets[1].Send(routerTable);
            }
            if (routerName.Equals("Router3"))
            {
                //routerSendingSockets[2].Client(localIP, 1);
                routerSendingSockets[2].Send(routerTable);
            }
            if (routerName.Equals("Router4"))
            {
                //routerSendingSockets[3].Client(localIP, 1);
                routerSendingSockets[3].Send(routerTable);
            }
            Console.WriteLine("Wysyłanie konfiguracji do " + routerName + " ...");
        }

        /**
        * metoda wysyłająca tablice routingową do hosta
        * @ hostName - string, nazwa docelowego hosta
        */
        private static void SendHostTable(string hostName)
        {   
            Console.WriteLine("Parosowanie konfiguracji dla " + hostName + " ...");
            StringBuilder builder = new StringBuilder();
            builder.Append("NMS;");
            builder.Append(ReadHostConfig(hostName));          
            string hostTable = builder.ToString();
            //sendingSocket.Client(localIP, 1);
            //sendingSocket.Send(routerTable);
            if (hostName.Equals("Host1"))
            {
                //hostSendingSockets[0].Client(localIP, 1);
                hostSendingSockets[0].Send(hostTable);
            }
            else if (hostName.Equals("Host2"))
            {
                //hostSendingSockets[1].Client(localIP, 1);
                hostSendingSockets[1].Send(hostTable);
            }

            Console.WriteLine("Wysyłanie konfiguracji do " + hostName + " ...");
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

            if(message.Contains("Host"))
            {
                Console.WriteLine("Otrzymano request od " + message + " o tabele hosta");
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

        private static void ParseLocalConfig() 
        {
            string localConfig = parser.ParseLocalConfig("local.xml");

            Console.WriteLine("sparsowany xml: "+ localConfig);
            String[] splittedConfig = localConfig.Split(',');
            routerSendingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[1])));
            routerSendingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[3])));
            routerSendingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[5])));
            routerSendingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[7])));
            hostSendingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[9])));
            hostSendingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[11])));
            routerReceivingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[13])));
            routerReceivingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[15])));
            routerReceivingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[17])));
            routerReceivingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[19])));
            hostReceivingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[21])));
            hostReceivingSockets.Add(new UDPSocket(Int32.Parse(splittedConfig[23])));
        }

        /**
         * metoda Systemu Zarządzania
         * @ args - tablica typu string
        */
        static void Main(string[] args)
        {
            string command;
        
            ManagementSystem.ShowInterface();
            ManagementSystem.ParseLocalConfig();
            ManagementSystem.StartServer();
            ManagementSystem.StartClient();
            Console.ReadLine();
            //while(true)
            //{
            //    Console.Write("NMS# ");
            //    command = Console.ReadLine();
            //    RunCommand(command);
                


            //}
            
        }
    }
}
