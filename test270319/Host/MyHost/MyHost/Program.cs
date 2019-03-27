using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPTestSend
{
    class Program
    {

        static void Main(string[] args)
        {
            Router router = new Router(11000, "192.168.1.255");
            router.SendMessage();
        }
    }
}

