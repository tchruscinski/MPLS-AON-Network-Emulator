using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Host
{
    //Klasa implementujacą hosta, wysyla i odbiera pakiety

    class Host
    {
        //private static Socket sokcet; //socket ktory wysyla pakiety do chmury
        //private static IPEndPoint endPoint; //reprezentuje punkt koncowy sieci jako adres ip i numer portu
        //private static IPAddress destinationIP; //adres ip do ktorego beda skierowane pakiety
        private string _name = " "; //nazwa hosta
        private UDPSocket sendingSocket = new UDPSocket(); //socket, ktory wysyla pakiety
        private int _sendingPort; // nr portu, ktorym sendingSocket wysyla pakiety
        private UDPSocket receivingSocket = new UDPSocket(); //sokcet, ktory nasluchuje na przychodzace pakiety
        private int _receivingPort; //nr portu, na ktorym receivingPort nasluchuje
        public Host(string name,int sendingPort, int receivingPort)
        {
            _name = name;
            _sendingPort = sendingPort;
            _receivingPort = receivingPort;
            sendingSocket.Client(Utils.destinationIP, _sendingPort, this);
            receivingSocket.Server(Utils.destinationIP, _receivingPort, this);
        }


        /*
         * Pobiera pakiet od socketu, usuwa naglowek pozostawiajac jedynie tresc wiadomosci
         * @ packet, tresc pakietu
         */
        public void ReadPacket(string packet)
        {
            Byte[] bytes = Encoding.ASCII.GetBytes(packet);
            //petla wyszukuje, na ktorym bajcie jest znak konca naglowka 
            //zeby usunac wszystkie bajty do niego wlacznie, pozostawiajac sama tresc otrzymanej wiadomosci
            int counter = 0;
            while (counter < bytes.Length && bytes[counter] != ';')
                counter++;

            counter++; //po wyjsciu z petli counter jest na bajcie konczacym naglowek
                           //ktory nie powinien byc wyswietlany
                           //po inkrementacji przechodzi na pierwszy bajt wiadomosci

            //zapisuje ilosc bajtow rowna (bytes.Length - counter), zaczynajac od indeksu counter
            //String message = Encoding.ASCII.GetString(bytes, counter, (bytes.Length - counter));
            Console.WriteLine(_name + " otrzymal:" + "message");
        }
        /*
         * Wysyla pakiet danych, dodajac w naglowku nazwe hosta docelowego
         * @ destinationHost, nazwa hosta docelowego, @ message, tresc wiadomosci
         */
        public void SendPacket(string destinationHost, string message)
        {
            StringBuilder builder = new StringBuilder(destinationHost);
            builder.Append(":"); //znak konca adresu docelowego
            //miedzy :, a ; jest miejsce na etykiety mpls
            builder.Append(";"); //znak konca naglowka
            builder.Append(message);
            sendingSocket.Send(builder.ToString());
        }   

        public string getName()
        {
            string name = this._name;
            return name;
        }

        /**
         * metoda konfigurująca parametry hosta:
         * @ portNumber port, na który będzie wysyłać pakiety 
         * @ targetIP IP docelowe
        */
        //private static void ConfigureHost(int portNumber, string targetIP)
        //{
        //    destinationIP = IPAddress.Parse(targetIP);
        //    endPoint = new IPEndPoint(destinationIP, portNumber);
        //}

        /**
         * metoda wysyłająca wiadomość
         * @ message - treść wiadomości
        */
        //private static void SendMessage(string message)
        //{
        //    //String text = "testtestest";
        //    byte[] sendbuf = Encoding.ASCII.GetBytes(message);

        //    socket.SendTo(sendbuf, endPoint);
        //    Console.WriteLine("Message sent to the broadcast address");
        //}

        /**
         * metoda zwracająca lokalny adres IP
         * @ no arguments, string return type
        */
        //private static string GetLocalIPAddress()
        //{
        //    if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
        //    {
        //        return null;
        //    }
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip.ToString();
        //        }
        //    }
        //    throw new Exception("No network adapters with an IPv4 address in the system!");
        //}

        /**
         * metoda parsująca argumenty metody Main Hosta, configurująca Hosta i wysyłająca wiadomość
         * @ args - tablica typu string
        */
        //private static void ParseAndExecuteForHostArguments(string[] args)
        //{
        //    int portNumber = Int32.Parse(args[0]);
        //    string targetIP = args[1];
        //    string message = args[2];

        //    Console.WriteLine(portNumber + " " + targetIP + " " + message);
        //    Console.ReadKey();

        //    //ConfigureHost(portNumber, targetIP);
        //    //SendMessage(message);
        //}

    }
}