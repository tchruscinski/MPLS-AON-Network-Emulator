using System;

namespace ConnectionCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            string userInput;
            UDPSocket s = new UDPSocket();
            s.Server("127.0.0.1", 27000);

            UDPSocket c = new UDPSocket();

            while (true)
            {
                
                c.Client("127.0.0.1", 27000);
                userInput = Console.ReadLine();
                c.Send(userInput);
            }

            //Console.ReadKey();
        }
    }
}