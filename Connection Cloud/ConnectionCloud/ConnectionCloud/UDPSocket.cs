using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;


namespace ConnectionCloud
{
    public class UDPSocket
    {
        private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;
        DateTime dt = DateTime.Now;
        static Time time = new Time();

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public void Server(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Created UDPServer at: " + address + ":" + port);
            Receive();
        }

        public void Connect(string address, int port)
        {
            _socket.Connect(IPAddress.Parse(address), port);
            Receive();
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " SEND: {0}, {1}", bytes, text);
            }, state);
        }

        private string Receive()
        {
             string msg = "";
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " RECV: {0}: {1}", epFrom.ToString(), bytes);
                msg = Encoding.ASCII.GetString(so.buffer, 0, bytes);
            }, state);

            return msg;
        }

        private string MessageProcessing(string message)
        {
            //TODO
            string proceeded_message = "";
            return proceeded_message;
        }

    }
}
