using System;

namespace ConnectionCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            string userInput;
            ConnectionCloud cc = new ConnectionCloud();
            string test;

            test = cc.ReadCloudConfig("2");
            Console.WriteLine("{0}", test);

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