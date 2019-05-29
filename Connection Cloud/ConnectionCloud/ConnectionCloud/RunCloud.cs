using System;
using System.Collections.Generic;

namespace ConnectionCloud
{
    class Program
    {
        static void Main(string[] args)
        {   
            ConnectionCloud cc = new ConnectionCloud();
            cc.StartText();

            CableEmulatorTableParser cb = new CableEmulatorTableParser();
            List<RoutingTableLine> rtl = new List<RoutingTableLine>();
            List<UDPSocket> udp_sock_recv = new List<UDPSocket>();
            List<UDPSocket> udp_sock_send = new List<UDPSocket>();

            rtl = cb.ParseCableCloudEmulatorTable();
            cc.AddRoutingTable(rtl);
            int SOCKET_PAIRS_NUMBER = cb.getNumberOfCables();

            for (int sock_number = 0; sock_number < SOCKET_PAIRS_NUMBER; sock_number++)
            {
                UDPSocket sockRecv = new UDPSocket();
                udp_sock_recv.Add(sockRecv);
                cc.AddReceivingSocket(udp_sock_recv[sock_number]);
                UDPSocket sockSend = new UDPSocket();
                udp_sock_send.Add(sockSend);
                cc.AddSendingSocket(udp_sock_send[sock_number]);
            }
            
            cc.StartSockets();
            Console.ReadKey();
            

        }

        
    }
}