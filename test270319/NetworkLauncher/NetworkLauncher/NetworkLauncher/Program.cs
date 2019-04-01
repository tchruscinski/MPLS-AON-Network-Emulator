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
            //Process.Start(@"C:\Users\Mafiu\Desktop\TSST\tsst-network-emulator\test270319\Cloud\Cloud\Cloud\bin\Debug\Cloud.exe");
            //Process.Start(@"C:\Users\Mafiu\Desktop\TSST\tsst-network-emulator\test270319\Host\MyHost\MyHost\bin\Debug\MyHost.exe");
            //Process.Start(@"C:\Users\Mafiu\Desktop\TSST\tsst-network-emulator\test270319\Host\MyHost\MyHost\bin\Debug\MyHost.exe");
            Process cloudProcess = new Process();
            Process host1Process = new Process();
            Process host2Process = new Process();

            cloudProcess.StartInfo.FileName = @"C:\\Users\Mafiu\Desktop\TSST\tsst-network-emulator\test270319\Host\MyHost\MyHost\bin\Debug\Cloud.exe";
            //           cloudProcess.StartInfo.FileName = "C:\Users\Mafiu\Desktop\TSST\tsst-network-emulator\test270319\Host\MyHost\MyHost\bin\Debug\MyHost.exe";
            //           cloudProcess.StartInfo.FileName = "C://Users/Mafiu/Desktop/TSST/tsst-network-emulator/test270319/Cloud/Cloud/Cloud\bin/Debug/Cloud.exe";
            cloudProcess.Start();
        }
    }
}
