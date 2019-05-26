using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class CommandLineInterface
    {
        private static void ShowHelp()
        {
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("---POMOC---");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("send [host_name] [message] || s [host_name] [message]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Przeslanie wiadomosci [message] do hosta [host_name]");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("quit || q");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Wylaczenie routera");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("connection_request [desination_host_name] || cq");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Wyslanie żądania zestawienia połączenia z danym hostem");

        }

        private static void WrongUsage()
        {
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Incorrect usage of CLI.");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Check help to get more info (\"help\")");
            Console.WriteLine(" ");
        }

        public void RunCommand(string input, Host host)
        {
            String[] comm;
            if (input == "help" || input == "h" || input == "?")
            {
                ShowHelp();

            }
            else
            {
                comm = input.Split(' ');

            
                if (comm[0] == "send" || comm[0] == "s")
                {
                    if (comm.Length >= 3)
                    {
                        StringBuilder messageBuilder = new StringBuilder();
                        for (int i = 2; i < comm.Length; i++)
                        {
                            messageBuilder.Append(comm[i]);
                            messageBuilder.Append(' ');
                        }
                        host.SendPacket(comm[1], messageBuilder.ToString());
                    }
                    else
                    {
                        WrongUsage();
                    }
                }
                
                else if (comm[0] == "quit" || comm[0] == "exit" || comm[0] == "q")
                {
                    Environment.Exit(0);
                }
                else if (comm[0] == "connection_request" || comm[0] == "cq")
                {
                    if (comm.Length == 2) host.SendConnectionRequest(comm[1]);
                    else WrongUsage();
                }
                else
                {
                    Console.WriteLine("Invalid command. Please use \"help\" to display help");
                }
            }
        }


    }
}
