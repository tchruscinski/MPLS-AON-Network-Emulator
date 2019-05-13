using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;



namespace RouterV1
{
    class UDPSocket
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;
        DateTime dt = DateTime.Now;
        static Time time = new Time();
        private String timeStamp = time.GetTimestamp(DateTime.Now);
        public int counter = 0;
        Router _router;
        private int _port;

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }


        public int getPort() { return _port; }

        public void SetPort(int portNumber) 
        {
            _port = portNumber;
        }

        public void Server(string address, int port, Router router)
        {
            _port = port;
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            _router = router;
            Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Created UDPServer at: " + address + ":" + port);
            Receive(_router);
        }
        public void Client(string address, int port, Router router)
        {
            _port = port;
            _socket.Connect(IPAddress.Parse(address), port);
            _router = router;
            Receive(_router);
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                timeStamp = time.GetTimestamp(DateTime.Now);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " SEND: bytes: [{0}], message: [{1}]", bytes, text);
                Console.ForegroundColor = ConsoleColor.Gray;
            }, state);
        }
        /*
         * Metoda nasluchuje na przychodzace pakiety
         * @ router, referencja do routera, zeby umozliwic przeslanie mu tresci pakietu
         */
        public void Receive(Router router)
        {

            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                try
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    timeStamp = time.GetTimestamp(DateTime.Now);
                    Console.WriteLine(timeStamp+" RECV: {0}: bytes: [{1}], message: [{2}]", epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));
                    router.SetIncPort(_port);
                    router.ReadPacket(Encoding.ASCII.GetString(so.buffer, 0, bytes));
                    counter++;
                }
                catch (SocketException e)
                {
                    timeStamp = time.GetTimestamp(DateTime.Now);
                    router.ActualizeNHFLETable(_port);
                    router.RefactorPacket();
                    Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Nie mozna nawiazac polaczenia");
                    //tutaj bedzie mozna wyslac wiadomosc do systemu zarzadzajacego, ze 
                    //host/router nie jest dostepny
                }
            }, state);



        }
    }

}

