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

            g.Server("127.0.0.1", 28012, cc);

            while (true)
            {
                userInput = Console.ReadLine();
                g.Connect("127.0.0.1", 27000, cc);
                g.Send(userInput);
            }
            
        }
    }
}