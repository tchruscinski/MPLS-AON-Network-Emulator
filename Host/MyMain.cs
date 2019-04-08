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
            string userInput;
            UDPSocket clientSocket = new UDPSocket();
            Host host1 = new Host("host1", 29001, 29002);
            clientSocket.Client("127.0.0.1", 27001, host1);
            

            while (true)
            {
                string command;
                String[] proccessedCommand;
                Console.Write(host1.getName() + "> ");
                command = Console.ReadLine();
                if(command == "?")
                {
                    BasicTerminal.GetHelp();
                }
                else
                {
                    proccessedCommand = command.Split(' ');
                    if(proccessedCommand.Length == 3)
                    {
                        if(proccessedCommand[0] == "send")
                        clientSocket.Send(command);
                        
                    }
                }
                //clientSocket.Send(msg);
            }
        }
    }
}
