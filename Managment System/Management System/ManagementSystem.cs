using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        /**
        * metoda parsująca plik konfiguracyjny dla sieci
        * @ no arguments, void
        */
        private void ReadConfig()
        {
            routersListeningPorts = parser.ParseConfig("Router", "ListeningPort");
            for(int i = 0; i < routersListeningPorts.Count; i++)
            {
                Console.WriteLine("port routera: "+routersListeningPorts[i]);
            }
        }

        /**
        * metoda startująca serwer na listeningSockecie systemu zarządzania
        * @ no arguments, void
        */
        private static void StartServer()
        {
            //sendingSocket.RunServer();
        }
        
        /**
        * metoda wyświetlająca interfejs systemu zarządznia
        * @ no arguments, void
        */
        private static void ShowInterface()
        {
            Console.WriteLine("System Zarządzania v1.0");

            Console.WriteLine("");
            StartServer();
        }

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
        }
    }
}
