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
            Host host1 = new Host("host1", 26999, 111);
            Host host2 = new Host("host2", 112, 27002);
            //Host host3 = new Host("host1", 111, 112);
            //Host host4 = new Host("host2", 112, 111);
            //Console.ReadLine();
            //host3.SendPacket("host2", "wiadomosc");

            host1.SendPacket("host2", "wiadomosc testowa");

            Console.ReadLine();
        }
    }
}
