using System;

namespace ConnectionCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            //string userInput;
            UDPSocket s = new UDPSocket();
            s.Server("127.0.0.1", 27000);
            
        }
    }
}