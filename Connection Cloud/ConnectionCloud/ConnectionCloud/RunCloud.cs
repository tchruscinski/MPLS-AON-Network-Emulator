﻿using System;
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

            a.Server("127.0.0.1", rtl[0]._incomingPort, cc);
            b.Server("127.0.0.1", rtl[1]._incomingPort, cc);
            c.Server("127.0.0.1", rtl[2]._incomingPort, cc);
            d.Server("127.0.0.1", rtl[3]._incomingPort, cc);

            e.Client("127.0.0.1", rtl[0]._outgoingPort);
            f.Client("127.0.0.1", rtl[1]._outgoingPort);
            g.Client("127.0.0.1", rtl[2]._outgoingPort);
            h.Client("127.0.0.1", rtl[3]._outgoingPort);

            Console.ReadKey();
            //while(true)
            //{
            //    Console.ReadKey();
            //}

        }
    }
}