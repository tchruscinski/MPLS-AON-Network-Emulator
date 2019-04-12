using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class BasicTerminal
    {
        public static void GetHelp()
        {
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-----Welcome to basic host CLI-----");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("  ");
            Console.WriteLine("Following commands are available:");
            Console.WriteLine("? - help - displays this message");
            Console.WriteLine("send [] [] [] - //TO DO");
            Console.WriteLine(" ");

        }
    }
}
