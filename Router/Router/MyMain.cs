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
            if (args.Length != 0)
            {
                Router router = new Router(args[0]);
                
                router.ManagementRequest();
                Console.ReadLine();
                //Console.WriteLine("NHLFE Table");
                //router.ShowNHLFETable();
                //Console.WriteLine("ILM Table");
                //router.ShowILMTable();
                Console.ReadLine();
                return;

            }
            //Router rt = new Router("Router1");
            //rt.ManagementRequest();
            //Router rter = new Router("Router3");
            //rter.ManagementRequest();
            ////rt.ParseLocalConfig();
            //Console.ReadLine();

            //// host1 <--> sendingRouter <--> midRouter <--> receivingRouter <--> host2

            //UDPSocket socket0 = new UDPSocket();
            //socket0.Server(Utils.destinationIP, 26999, sendingRouter);
            //UDPSocket socket3 = new UDPSocket();
            ////socket3.Client(Utils.destinationIP, 21370, sendingRouter);
            //socket3.Client(Utils.destinationIP, 27000, sendingRouter);
            //ILMLine ilm0 = new ILMLine(26999, 17, "", 1);
            //sendingRouter.AddSendingSocket(socket3);
            //sendingRouter.AddReceivingSocket(socket0);
            //sendingRouter.AddILMLine(ilm0);


            //NHLFELine nhlfe1 = new NHLFELine(1, Action.PUSH, 20, 27000, 0); //wyslij portem 2700 z etykieta 20
            //sendingRouter.AddNHLFELine(nhlfe1);
            //sendingRouter.ShowNHLFETable();

            //Router midRouter = new Router("midRouter");
            //UDPSocket socket = new UDPSocket();
            //UDPSocket socket4 = new UDPSocket();
            //socket4.Client(Utils.destinationIP, 27001, midRouter);
            //socket.Server(Utils.destinationIP, 27000, midRouter);
            //NHLFELine nhlfe2 = new NHLFELine(1, Action.SWAP, 30, 27001, 0);
            //ILMLine ilm1 = new ILMLine(27000, 20, "", 1);
            //midRouter.AddILMLine(ilm1);
            //midRouter.AddReceivingSocket(socket);
            //midRouter.AddNHLFELine(nhlfe2);
            //midRouter.AddSendingSocket(socket4);

            //Router receivingRouter = new Router("receivingRouter");
            //UDPSocket socket2 = new UDPSocket();
            //UDPSocket socket5 = new UDPSocket();
            //socket5.Client(Utils.destinationIP, 29002, receivingRouter);
            //socket2.Server(Utils.destinationIP, 27001, receivingRouter);
            //ILMLine ilm2 = new ILMLine(27001, 30, "", 1);
            //ILMLine ilm3 = new ILMLine(27001, 17, "30", 2);
            //ILMLine ilm4 = new ILMLine(27001, 17, "", 3);
            //NHLFELine nhlfe3 = new NHLFELine(1, Action.POP, 0, 0, 0);
            //NHLFELine nhlfe4 = new NHLFELine(2, Action.PUSH, 31, 29002, 0);
            //NHLFELine nhlfe5 = new NHLFELine(3, Action.PUSH, 35, 29002, 0);
            //receivingRouter.AddILMLine(ilm2);
            //receivingRouter.AddILMLine(ilm3);
            //receivingRouter.AddILMLine(ilm4);
            //receivingRouter.AddNHLFELine(nhlfe3);
            //receivingRouter.AddNHLFELine(nhlfe4);
            //receivingRouter.AddNHLFELine(nhlfe5);
            //receivingRouter.AddReceivingSocket(socket2);
            //receivingRouter.AddSendingSocket(socket5);



            //for (int i = 0; i < 100; i++)
            //    socket1.Send(i.ToString());
            //sendingRouter.SendPacket("host2;tresc wiadomosci.......sasgg", 27000);
            //sendingRouter.SendPacket();


            //Tomek test połączenie router NMS
            //sendingRouter.SetReceivingManagementSocket(1);
            //sendingRouter.SetSendingManagementSocket(100);
            //sendingRouter.ManagementRequest();
            //Console.ReadLine();
            //-----------


            //Console.ReadLine();
            //Console.WriteLine("Host: " + midRouter.GetDestinationHost());
            //Console.WriteLine(midRouter.GetName());

            //sendingRouter.ShowNHLFETable();
            //Console.ReadLine();
            //Console.WriteLine("Host: " + receivingRouter.GetDestinationHost());
            //Console.WriteLine(receivingRouter.GetName());


            //Console.WriteLine("Odebrano wiadomosci socket: " + socket.counter);
            //Console.ReadKey();

            //Console.WriteLine("Odebrano wiadomosci socket2: " + socket2.counter);
            //Console.ReadKey();


        }
    }
}