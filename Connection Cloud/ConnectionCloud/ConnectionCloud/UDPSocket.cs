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
        ConnectionCloud _connectionCloud;
        public int _port;

        public int getPort() { return _port; }

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public void Server(string address, int port, ConnectionCloud connectionCloud) 
        {
            try
            {
                _port = port;
                _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
                _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Created UDPServer at: " + address + ":" + port);
                _connectionCloud = connectionCloud;
                AsyncTransfer(_connectionCloud);
            } catch(SocketException e)
            {
                Console.WriteLine("Nieprawidlowa konfiguracja chmury");
                Console.WriteLine("Błąd: " + e.Message);
                return;
            }
              
        }
        //W celu testów tylko
        public void Client(string address, int port)
        {
        
        try
            {
                _port = port;
                //_socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
                //_socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
                _socket.Connect(address, port);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Created UDPClient at: " + address + ":" + port);
                Console.ForegroundColor = ConsoleColor.Gray;
                Receive();
            } 
        catch (SocketException e)
            {
                Console.WriteLine("Nieprawidlowa konfiguracja chmury");
                Console.WriteLine("Błąd: " + e.Message);
                return;
            }
            


        }

        public void Connect(string address, int port)
        {
            // _socket.Connect(address, port);
            _socket.Connect("127.0.0.1", port);
            Receive();
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
            }, state);
        }

        private void Receive()
        {
            //Console.WriteLine("UDPReceiver");
             string msg = "";
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                try
                {
                    State so = (State)ar.AsyncState;
                    int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                    _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Received message: {0}: {1}", _port, epFrom.ToString());
                    Console.ForegroundColor = ConsoleColor.Gray;
                    msg = Encoding.ASCII.GetString(so.buffer, 0, bytes);
                } catch(SocketException e)
                {
                    Console.WriteLine("Nieprawidlowa konfiguracja chmury");
                    Console.WriteLine("Błąd: " + e.Message);
                    return;
                }
            }, state);
        }

        private void AsyncTransfer(ConnectionCloud connectionCloud)
        {
            string msg = "";
            
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Received message: {0}: {1}", _port, epFrom.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
                msg = Encoding.ASCII.GetString(so.buffer, 0, bytes);
                connectionCloud.Proceed(msg, this._port);
                
            }, state);
        }

    }
}
