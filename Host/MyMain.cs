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
                string msg;
                msg = Console.ReadLine();
                clientSocket.Send(msg);
            }
        }
    }
}
