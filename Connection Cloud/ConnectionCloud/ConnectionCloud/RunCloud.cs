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
            //UDPSocket m = new UDPSocket();
            //UDPSocket n = new UDPSocket();


            UDPSocket a1 = new UDPSocket();
            UDPSocket b1 = new UDPSocket();
            UDPSocket c1 = new UDPSocket();
            UDPSocket d1 = new UDPSocket();
            UDPSocket e1 = new UDPSocket();
            UDPSocket f1 = new UDPSocket();
            UDPSocket g1 = new UDPSocket();
            UDPSocket h1 = new UDPSocket();
            UDPSocket i1 = new UDPSocket();
            UDPSocket j1 = new UDPSocket();
            UDPSocket k1 = new UDPSocket();
            UDPSocket l1 = new UDPSocket();
            //UDPSocket m1 = new UDPSocket();
            //UDPSocket n1 = new UDPSocket();

            cc.AddReceivingSocket(a);
            cc.AddReceivingSocket(b);
            cc.AddReceivingSocket(c);
            cc.AddReceivingSocket(d);
            cc.AddReceivingSocket(e);
            cc.AddReceivingSocket(f);
            cc.AddReceivingSocket(g);
            cc.AddReceivingSocket(h);
            cc.AddReceivingSocket(i);
            cc.AddReceivingSocket(j);
            cc.AddReceivingSocket(k);
            cc.AddReceivingSocket(l);
            //cc.AddReceivingSocket(m);
            //cc.AddReceivingSocket(n);

            cc.AddSendingSocket(a1);
            cc.AddSendingSocket(b1);
            cc.AddSendingSocket(c1);
            cc.AddSendingSocket(d1);
            cc.AddSendingSocket(e1);
            cc.AddSendingSocket(f1);
            cc.AddSendingSocket(g1);
            cc.AddSendingSocket(h1);
            cc.AddSendingSocket(i1);
            cc.AddSendingSocket(j1);
            cc.AddSendingSocket(k1);
            cc.AddSendingSocket(l1);
            //cc.AddSendingSocket(m1);
            //cc.AddSendingSocket(n1);

            cc.StartSockets();



            Console.ReadKey();
            //while(true)
            //{
            //    Console.ReadKey();
            //}

        }
    }
}