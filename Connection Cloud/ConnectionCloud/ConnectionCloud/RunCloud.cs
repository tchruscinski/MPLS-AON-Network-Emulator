using System;
using System.Collections.Generic;

namespace ConnectionCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            string userInput;
            ConnectionCloud cc = new ConnectionCloud();
            CableEmulatorTableParser cb = new CableEmulatorTableParser();

            List<RoutingTableLine> rtl = new List<RoutingTableLine>();

            rtl = cb.ParseCableCloudEmulatorTable();
            //Console.WriteLine("{0}", RoutingTableLine);
            cc.AddRoutingTable(rtl);
            //UDPSocket s = new UDPSocket();
            //s.Server("127.0.0.1", 21370, cc);
            UDPSocket a = new UDPSocket();
            UDPSocket b = new UDPSocket();
            UDPSocket c = new UDPSocket();
            UDPSocket d = new UDPSocket();
            UDPSocket e = new UDPSocket();
            UDPSocket f = new UDPSocket();
            UDPSocket g = new UDPSocket();
            UDPSocket h = new UDPSocket();

            cc.AddReceivingSocket(a);
            cc.AddReceivingSocket(b);
            cc.AddReceivingSocket(c);
            cc.AddReceivingSocket(d);
            cc.AddSendingSocket(e);
            cc.AddSendingSocket(f);
            cc.AddSendingSocket(g);
            cc.AddSendingSocket(h);
            cc.StartSockets();


            Console.ReadKey();
            //while(true)
            //{
            //    Console.ReadKey();
            //}

        }
    }
}