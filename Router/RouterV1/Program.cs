using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    class Program
    {
        static void Main(string[] args)
        {
            Router router1 = new Router();
            RoutingLine line1 = new RoutingLine(27000);
            router1.AddRoutingLine(line1);
            UDPSocket socket1 = new UDPSocket();
            socket1.Client("127.0.0.1", 27000);
            for (int i = 0; i < 2000; i++)
                socket1.Send(i.ToString());


        }
    }
}
