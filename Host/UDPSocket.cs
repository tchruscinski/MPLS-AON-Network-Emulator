using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;



namespace Host
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
        Host _host;
        private int _port;

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }


        public int getPort() { return _port; }

        public void Server(string address, int port, Host host)
        {
            _port = port;
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            _host = host;
            Console.WriteLine("Created UDPServer at: " + address + ":" + port);
            Receive(_host);
        }
        public void Client(string address, int port, Host host)
        {
            _port = port;
            _socket.Connect(IPAddress.Parse(address), port);
            _host = host;
            Receive(_host);
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, state);
        }
        /*
         * Metoda nasluchuje na przychodzace pakiety
         * @ router, referencja do routera, zeby umozliwic przeslanie mu tresci pakietu
         */
        public void Receive(Host host)
        {
            try
            {
                _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    timeStamp = time.GetTimestamp(DateTime.Now);
                    //Console.WriteLine("RECV: {0}: {1}, {2}" + " at: " + timeStamp, epFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes));
                    host.ReadPacket(Encoding.ASCII.GetString(so.buffer, 0, bytes));
                    counter++;
                }, state);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Nie mozna nawiazac polaczenia");
                //tutaj bedzie mozna wyslac wiadomosc do systemu zarzadzajacego, ze 
                //host/router nie jest dostepny


            }
        }

    }
}
