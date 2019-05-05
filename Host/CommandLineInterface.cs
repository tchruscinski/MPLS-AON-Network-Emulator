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

            
                if (comm[0] == "send" || comm[0] == "drc")
                {
                    if (comm.Length == 3)
                    {
                        host.SendPacket(comm[1], comm[2]);
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
                else
                {
                    Console.WriteLine("Invalid command. Please use \"help\" to display help");
                }
            }
        }


    }
}
