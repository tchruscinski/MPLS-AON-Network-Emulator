using System;

namespace ConnectionCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            string userInput;
            ConnectionCloud cc = new ConnectionCloud();

            UDPSocket s = new UDPSocket();
            s.Server("127.0.0.1", 27000, cc);
            UDPSocket g = new UDPSocket();
            g.Client("127.0.0.1", 27098);

            while (true)
            {
               
                string msg;
                msg = Console.ReadLine();
                g.Connect("127.0.0.1", 27000);
                g.Send(msg);
            }
            
        }
    }
}