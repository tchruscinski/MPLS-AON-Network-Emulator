using System;
using System.Collections.Generic;

namespace ConnectionCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--------------TSST - CABLE CLOUD EMULATOR------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
            ConnectionCloud cc = new ConnectionCloud();
            CableEmulatorTableParser cb = new CableEmulatorTableParser();

            List<RoutingTableLine> rtl = new List<RoutingTableLine>();

            rtl = cb.ParseCableCloudEmulatorTable();
            cc.AddRoutingTable(rtl);
            UDPSocket a = new UDPSocket();
            UDPSocket b = new UDPSocket();
            UDPSocket c = new UDPSocket();
            UDPSocket d = new UDPSocket();
            UDPSocket e = new UDPSocket();
            UDPSocket f = new UDPSocket();
            UDPSocket g = new UDPSocket();
            UDPSocket h = new UDPSocket();
            UDPSocket i = new UDPSocket();
            UDPSocket j = new UDPSocket();
            UDPSocket k = new UDPSocket();
            UDPSocket l = new UDPSocket();

            cc.AddReceivingSocket(a);
            cc.AddReceivingSocket(b);
            cc.AddReceivingSocket(c);
            cc.AddReceivingSocket(d);
            cc.AddReceivingSocket(e);
            cc.AddReceivingSocket(f);
            
            cc.AddSendingSocket(g);
            cc.AddSendingSocket(h);
            cc.AddSendingSocket(i);
            cc.AddSendingSocket(j);
            cc.AddSendingSocket(k);
            cc.AddSendingSocket(l);

            cc.StartSockets();



            Console.ReadKey();
            //while(true)
            //{
            //    Console.ReadKey();
            //}

        }
    }
}