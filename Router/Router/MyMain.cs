using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    class MyMain
    {
        static void Main(string[] args)
        {
            // host1 <--> sendingRouter <--> midRouter <--> receivingRouter <--> host2
            Router sendingRouter = new Router("sendingRouter");
            UDPSocket socket0 = new UDPSocket();
            socket0.Server(Utils.destinationIP, 26999, sendingRouter);
            IPLine line1 = new IPLine(27000, "host2");
            MPLSLine mLine1 = new MPLSLine("host2", 0);
            MPLSLine mLine2 = new MPLSLine("host3", 1);
            sendingRouter.AddRoutingLine(line1);
            sendingRouter.AddRoutingLineMPLS(mLine1);
            sendingRouter.AddRoutingLineMPLS(mLine2);
            sendingRouter.AddReceivingSocket(socket0);

            FTNLine ftn1 = new FTNLine(1, 1);
            //sendingRouter.AddFTNLine(ftn1);
            NHLFELine nhlfe1 = new NHLFELine(1, Action.PUSH, 20, 27000, 0); //wyslij portem 2700 z etykieta 20
            sendingRouter.AddNHLFELine(nhlfe1);

            Router midRouter = new Router("midRouter");
            UDPSocket socket = new UDPSocket();
            socket.Server(Utils.destinationIP, 27000, midRouter);
            IPLine line2 = new IPLine(27001, "host2");
            midRouter.AddReceivingSocket(socket);
            midRouter.AddRoutingLine(line2);
            sendingRouter.AddRoutingLineMPLS(mLine1);

            Router receivingRouter = new Router("receivingRouter");
            UDPSocket socket2 = new UDPSocket();
            socket2.Server(Utils.destinationIP, 27001, receivingRouter);
            IPLine line3 = new IPLine(27002, "host2");
            receivingRouter.AddRoutingLine(line3);
            sendingRouter.AddRoutingLineMPLS(mLine1);
            receivingRouter.AddReceivingSocket(socket2);


            //for (int i = 0; i < 100; i++)
            //    socket1.Send(i.ToString());
            //sendingRouter.SendPacket("host2;tresc wiadomosci.......sasgg", 27000);
            //sendingRouter.SendPacket();

            Console.ReadLine();
            Console.WriteLine("Host: " + midRouter.GetDestinationHost());
            Console.WriteLine(midRouter.GetName());


            Console.ReadLine();
            Console.WriteLine("Host: " + receivingRouter.GetDestinationHost());
            Console.WriteLine(receivingRouter.GetName());


            Console.WriteLine("Odebrano wiadomosci socket: " + socket.counter);
            Console.ReadKey();

            Console.WriteLine("Odebrano wiadomosci socket2: " + socket2.counter);
            Console.ReadKey();


        }
    }
}