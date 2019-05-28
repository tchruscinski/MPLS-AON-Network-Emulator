using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    class MyMain
    {      
       static void Main(string[] args)
        {
            //Link link = new Link("Router1", "Router2", 1000, 10000);
            Node node1 = new Node("Node1");
            //Console.ReadLine();
            ////node1.SendPacket("test", 10);
            //foreach (UDPSocket i in node1.GetReceivingSockets())
            //    Console.WriteLine(i.getPort());
            Console.ReadLine();
            if (args.Length != 0)
            {
                Node node = new Node(args[0]);

                string command;

                CommandLineInterface cli = new CommandLineInterface();

                while (true)
                {
                    Console.Write("RouterCLI# ");
                    command = Console.ReadLine();
                    cli.RunCommand(command, node);
                }
            }


        
        }
    }
}