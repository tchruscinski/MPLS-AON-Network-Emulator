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
        //private static void compileProject()
        //{

        //}

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
            ConfigureHosts();
        }
    }
}
