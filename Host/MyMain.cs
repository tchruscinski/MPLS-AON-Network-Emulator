using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class MyMain
    {
        public static void Main(string[] args)
        {
            
            if (args.Length != 0)
            {
                Host host = new Host(args[0]);
            }
            UDPSocket s1 = new UDPSocket();
            UDPSocket s2 = new UDPSocket();
            Host host1 = new Host("host");
            Host host2 = new Host("host2");
            s1.Client("127.0.0.1", 1, host1);
            s2.Server("127.0.0.1", 2, host2);
            Console.ReadKey();
            s1.Send("chuj");
            Console.ReadLine();
            //string userInput;
            //UDPSocket clientSocket = new UDPSocket();
            //Host host1 = new Host("host1", 26999, 29002);
            ////Host host2 = new Host("host2", 1000, 10001);
            //MPLSLine mpls1 = new MPLSLine("host3", 1);
            //host1.AddRoutingLineMPLS(mpls1);
            //NHLFELine nhlfe1 = new NHLFELine(1, 17, 0);
            //ILMLine ilm1 = new ILMLine(31, "host1");
            //host1.AddNHLFELine(nhlfe1);
            //host1.AddILMLIne(ilm1);
            //clientSocket.Client("127.0.0.1", 27001, host1);
            //host1.SendPacket("host3", "wiadomosc testowa");
            //Console.ReadLine();
            //while (true)
            //{
            //    string command;
            //    String[] proccessedCommand;
            //    Console.Write(host1.getName() + "> ");
            //    command = Console.ReadLine();
            //    if(command == "?")
            //    {
            //        BasicTerminal.GetHelp();
            //    }
            //    else
            //    {
            //        proccessedCommand = command.Split(' ');
            //        if(proccessedCommand.Length == 3)
            //        {
            //            if(proccessedCommand[0] == "send")
            //            clientSocket.Send(command);

            //        }
            //    }
            //    //clientSocket.Send(msg);
            //}
        }
    }
}
