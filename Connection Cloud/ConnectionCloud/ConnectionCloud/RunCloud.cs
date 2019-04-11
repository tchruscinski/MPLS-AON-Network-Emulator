using System;

namespace ConnectionCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            string userInput;
            ConnectionCloud cc = new ConnectionCloud();
            CableEmulatorTableParser cb = new CableEmulatorTableParser();
            string test;

            test = cb.ParseCableCloudEmulatorTable();
            Console.WriteLine("{0}", test);
            Console.ReadKey();

            //UDPSocket s = new UDPSocket();
            //s.Server("127.0.0.1", 21370, cc);
            //UDPSocket g = new UDPSocket();
            //g.Client("127.0.0.1", 27098);

            //while(true)
            //{
            //    Console.ReadKey();
            //}

        }
    }
}