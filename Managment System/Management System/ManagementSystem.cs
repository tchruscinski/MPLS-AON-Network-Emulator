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
        static Time time = new Time();
        private static Parser parser = new Parser();
        private static int portNumber = 100;
        private static int connectionCloudListeningPort = 100;
        private static string localIP = "127.0.0.1"; //docelowe ip;
        private static int ConfigNb = 1;
        private static List<UDPSocket> routerSendingSockets = new List<UDPSocket>();
        private static List<UDPSocket> routerReceivingSockets = new List<UDPSocket>();
        private static List<UDPSocket> hostSendingSockets = new List<UDPSocket>();
        private static List<UDPSocket> hostReceivingSockets = new List<UDPSocket>();

        /**
        * Metoda parsująca plik konfiguracyjny dla routerow
        * @ routerName - string, nazwa docelowego routera
        */
        private static string ReadRouterConfig(string routerName, Boolean isChangeScenario)
        {
            //Console.WriteLine("przekazana nazwa "+routerName);
            StringBuilder builder = new StringBuilder();
            builder.Append("routers_config");
            if(isChangeScenario)
            {
                builder.Append(routerName[routerName.Length - 1]);
                routerName = routerName.Remove(routerName.Length - 1);
            }
            builder.Append(".xml");
            string configName = builder.ToString();
            //Console.WriteLine("nazwa szukana "+routerName+" i nazwa pliku: "+configName);
            string routerConfig = parser.ParseRouterTable(configName, routerName);
            Console.WriteLine("routerConfig: " + routerConfig);
            return routerConfig;
        }

        private static void DisplayRouterConfig(string routerName)
        {
            string routerConf = "";
            routerConf = ReadRouterConfig(routerName, false);
            try
            {
                IEnumerable<Tuple<string, string, string, string, string, string, string>> routerConfiguration;
                String[] splittedConfig = routerConf.Split(',');

                //zdaje sobie sprawe z tego ze to jest paskudnie zahardkodowane ale nie mam czasu rozkminiac ladniejszego rozwiazania :D 
                int i = 0;
                routerConfiguration =
                       new[]
                       {
                          Tuple.Create(splittedConfig[i], splittedConfig[i + 1], splittedConfig[i + 2], splittedConfig[i + 3],
                          splittedConfig[i + 4], splittedConfig[i + 5], splittedConfig[i + 6]),
                          Tuple.Create(splittedConfig[i+9], splittedConfig[i + 1+9], splittedConfig[i + 2+9], splittedConfig[i + 3+9],
                          splittedConfig[i + 4+9], splittedConfig[i + 5+9], splittedConfig[i + 6+9]),
                          Tuple.Create(splittedConfig[i+18], splittedConfig[i + 1+18], splittedConfig[i + 2+18], splittedConfig[i + 3+18],
                          splittedConfig[i + 4+18], splittedConfig[i + 5+18], splittedConfig[i + 6+18]),
                          Tuple.Create(splittedConfig[i+27], splittedConfig[i + 1+27], splittedConfig[i + 2+27], splittedConfig[i + 3+27],
                          splittedConfig[i + 4+27], splittedConfig[i + 5+27], splittedConfig[i + 6+27]),
                       };

                Console.WriteLine(routerConfiguration.ToStringTable(
                    new[] { "NHLFE_ID_MPLS", "Action", "Out Label", "OutPortN", "IncPort", "IncLabel", "PLS" },
                       a => a.Item1, a => a.Item2, a => a.Item3, a => a.Item4, a => a.Item5, a => a.Item6, a => a.Item7));
               
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Configuration for router {0} doesn't exist.", routerName);
            }

        }

        private static void DisplayHostConfig(string hostName)
        {
            string hostConf = "";
            hostConf = ReadHostConfig(hostName, false);

            try
            {
                IEnumerable<Tuple<string, string, string, string, string, string, string>> hostConfiguration;
                String[] splittedConfig = hostConf.Split(',');
                
                //zdaje sobie sprawe z tego ze to jest paskudnie zahardkodowane ale nie mam czasu rozkminiac ladniejszego rozwiazania :D 

                int i = 0;
                hostConfiguration =
                       new[]
                       {
                          Tuple.Create(splittedConfig[i], splittedConfig[i + 1], splittedConfig[i + 2], splittedConfig[i + 3],
                          splittedConfig[i + 4], splittedConfig[i + 5], splittedConfig[i + 6]),
                       };

                Console.WriteLine(hostConfiguration.ToStringTable(

                    new[] { "Destination Host", "NHLFE_ID", "Label", "Sender", "ID", "NLabel", "NextID" },
                       a => a.Item1, a => a.Item2, a => a.Item3, a => a.Item4, a => a.Item5, a => a.Item6, a => a.Item7));

            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Configuration for host {0} doesn't exist or has incorrect format.", hostName);
            }

        }

        /**
        * Metoda parsująca plik konfiguracyjny dla hostow
        * @ hostName - string, nazwa docelowego hosta
        */
        private static string ReadHostConfig(string hostName, Boolean isChangeScenario)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("host_config");
            if(isChangeScenario)
            {
                builder.Append(hostName[hostName.Length - 1]);
                hostName = hostName.Remove(hostName.Length - 1);
            }
            builder.Append(".xml");
            string configName = builder.ToString();
            string hostConfig = parser.ParseHostTable(configName, hostName);
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
            builder.Append(ReadRouterConfig(routerName, false));          
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
        * metoda wysyłająca tablice routingową do routera
        * @ routerName - string, nazwa docelowego routera
        */
        private static void SendRouterTable(string routerName, string scenarioNumber)
        {   
            Console.WriteLine("Parsowanie konfiguracji dla " + routerName + " ...");
            StringBuilder builder = new StringBuilder();
            builder.Append("NMS;");
            builder.Append(ReadRouterConfig(routerName + scenarioNumber, true));          
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
            Console.WriteLine("Parsowanie konfiguracji dla " + hostName + " ...");
            StringBuilder builder = new StringBuilder();
            builder.Append("NMS;");
            builder.Append(ReadHostConfig(hostName, false));          
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
        * metoda wysyłająca tablice routingową do hosta
        * @ hostName - string, nazwa docelowego hosta
        */
        private static void SendHostTable(string hostName, string scenarioNumber)
        {   
            Console.WriteLine("Parsowanie konfiguracji dla " + hostName + " ...");
            StringBuilder builder = new StringBuilder();
            builder.Append("NMS;");
            builder.Append(ReadHostConfig(hostName + scenarioNumber, true));          
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
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + "Otrzymano request od " + message + " o tabele hosta");
                SendHostTable(message);
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + "Tabela wysłana do " + message);
            }

            if(message.Contains("Router"))
            {
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + "Otrzymano request od " + message + " o tabele routera");
                SendRouterTable(message);
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + "Tabela wysłana do " + message);
            }   
        }

         /**
         * Metoda czytająca komendy z konsoli
         * @ no arguments, void
         * 
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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("change-config [router_name/host_name] [scenario_number] || cc [router_name/host_name] [scenario_number]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Zmiana konfiguracji routera na podstawie wybranego pliku XML");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("display-remote-config [router|host] [router|host_name] || drc [r|h] [router|host_name]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Wyswietlenie konfiguracji zdalnego urzadzenia");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("change-scenario [scenario_number] || chs [scenario_number]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Zmiana scenariusza (numer configa dla hosta o routera)");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("display-local-config [router_name] || dlc [router_name]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Wyswietlenie konfiguracji lokalnej");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("quit || q");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Wyjscie z NMS");

        }

        private static void WrongUsage()
        {
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Incorrect usage of CLI.");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Check help to get more info (\"help\")");
            Console.WriteLine(" ");
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

                if (comm[0] == "change-config" || comm[0] == "cc")
                {
                    if (comm.Length == 3)
                    {
                        if (comm[1].Contains("Host"))
                        {
                            SendHostTable(comm[1], comm[2]);
                        }
                        else if (comm[1].Contains("Router"))
                        {
                            SendRouterTable(comm[1], comm[2]);
                        }
                        else
                        {
                            WrongUsage();
                        }

                    }
                }
                else if (comm[0] == "display-local-config" || comm[0] == "dlc")
                {
                    Console.WriteLine(" ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("NMS local configuration:");
                    DisplayLocalConfig();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" ");
                }
                else if (comm[0] == "change-scenario" || comm[0] == "chs")
                {
                    if (comm.Length == 2)
                    {
                        Int32.TryParse(comm[1], out ConfigNb);
                        Console.WriteLine("co to tam wyszlo:" + ConfigNb);
                        SendRouterTable("Router1");
                        SendRouterTable("Router2");
                        SendRouterTable("Router3");
                        SendRouterTable("Router4");
                        SendHostTable("Host1");
                        SendHostTable("Host2");
                    }
                }
                else if (comm[0] == "display-remote-config" || comm[0] == "drc")
                {
                    if (comm.Length == 3)
                    {
                        if (comm[1].Contains("Host"))
                        {
                            DisplayHostConfig(comm[2]);
                        }
                        else if (comm[1].Contains("Router"))
                        {
                            DisplayRouterConfig(comm[2]);
                        }
                        else
                        {
                            WrongUsage();
                        }
                    }
                    else
                    {
                        WrongUsage();
                    }
                }
                else if (comm[0] == "run-scenario" || comm[0] == "rs")
                {
                    Console.WriteLine("TODO");
                }
                else if (comm[0] == "quit" || comm[0] == "exit" || comm[0] == "q")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid command. Please use \"help\" to display help");
                }   
            }
        }

        public static void DisplayLocalConfig()
        {
            string localConfig = parser.ParseLocalConfig("local.xml");
            String[] splittedConfig = localConfig.Split(',');
            for(int i = 0; i < splittedConfig.Length; i=i+2 )
            {
                Console.WriteLine("role: [{0}] port: [{1}]", splittedConfig[i], splittedConfig[i + 1]);
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
            while (true)
            {
                Console.Write("NMS# ");
                command = Console.ReadLine();
                RunCommand(command);
            }

        }
    }
}
