using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace NetworkLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Tymczasowo statyczna sciezka, to trzeba bedzie poprawic
             * Czasami nie przechwytuje pakietu, bedzie trzeba dodac jakis czas oczekiwania 
             * miedzy odpaleniem kolejnych aplikacji
             */
            Process.Start(@"C:\Users\Mafiu\Desktop\TSST\tsst-network-emulator\test270319\NetworkLauncher\NetworkLauncher\Pliki wykonywalne\Cloud\setup.exe");
            Process.Start(@"C:\Users\Mafiu\Desktop\TSST\tsst-network-emulator\test270319\NetworkLauncher\NetworkLauncher\Pliki wykonywalne\Host\setup.exe");



        }
    }
}
