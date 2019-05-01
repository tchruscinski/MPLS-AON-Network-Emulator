using System;
using System.Collections.Generic;

namespace ConnectionCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            int RECV_SOCK_NUMBER = 12;
            int SEND_SOCK_NUMBER = 12;
            
            ConnectionCloud cc = new ConnectionCloud();
            cc.StartText();
            CableEmulatorTableParser cb = new CableEmulatorTableParser();
            List<RoutingTableLine> rtl = new List<RoutingTableLine>();
            List<UDPSocket> udp_sock_recv = new List<UDPSocket>();
            List<UDPSocket> udp_sock_send = new List<UDPSocket>();

            rtl = cb.ParseCableCloudEmulatorTable();
            cc.AddRoutingTable(rtl);

            for (int sock_number = 0; sock_number < RECV_SOCK_NUMBER; sock_number++)
            {
                UDPSocket sock = new UDPSocket();
                udp_sock_recv.Add(sock);
            }

            for (int send_sock_number = 0; send_sock_number < SEND_SOCK_NUMBER; send_sock_number++)
            {
                UDPSocket sock = new UDPSocket();
                udp_sock_send.Add(sock);
            }

           
            for(int sock_number = 0; sock_number < RECV_SOCK_NUMBER; sock_number++)
            {
                cc.AddReceivingSocket(udp_sock_recv[sock_number]);
            }

            for(int sock_number = 0; sock_number <SEND_SOCK_NUMBER; sock_number++)
            {
                cc.AddSendingSocket(udp_sock_send[sock_number]);
            }
            
            cc.StartSockets();
            Console.ReadKey();
            

        }

        
    }
}