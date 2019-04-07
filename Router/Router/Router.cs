using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    /*
     * Klasa reprezentuje wezel kliencki, odbiera pakiet, 
     * sprawdza w tablicy routingowej, gdzie kierowac go dalej, po czym wysyla go odpowiednim portem
     * 
     * TODO: Etykiety MPLS
     */
    class Router
    {
        private string _name = " "; //nazwa routera
        private List<RoutingLine> tableIP_FIB = new List<RoutingLine>(); //tablica routingowa IP
        private List<RoutingLineMPLS> tableMPLS_FIB = new List<RoutingLineMPLS>(); //tablica routingowa MPLS
        //sockety, ktorymi pakiety sa przesylane dalej
        private List<UDPSocket> sendingSockets = new List<UDPSocket>();
        //sockety, ktore odbieraja pakiety
        private List<UDPSocket> receivingSockets = new List<UDPSocket>();
        private String _packet = " "; //tresc pakietu obslugiwanego w danym momencie przez router,
        private String destinationHost = " "; //docelowy host pakietu obslugiwanego w danym momencie

        public Router(string name)
        {
            _name = name;
        }
        public string GetName() { return _name; }

        /*
         * Metoda dodaje 
         * @ newLine, nowy wiersz do tablicy routingowej 
         * oraz tworzy UDP socket nasluchujacy na tym porcie
         */
        public void AddRoutingLine(RoutingLine newLine)
        {
            tableIP_FIB.Add(newLine);
            UDPSocket socket = new UDPSocket();
            socket.Client(Utils.destinationIP, newLine.GetPort(), this);
            //socket.Server(Utils.destinationIP, 27000, this);
            sendingSockets.Add(socket);
        }

        public void AddRoutingLineMPLS(RoutingLineMPLS newLine) { tableMPLS_FIB.Add(newLine); }

        public string GetDestinationHost() { return destinationHost; }
        /*
         * Metoda dodaje,
         * @ newSocket, nowy socket do listy
         */
        public void AddReceivingSocket(UDPSocket newSocket)
        {
            receivingSockets
                .Add(newSocket);
        }
        /*
        * Metoda dodaje,
        * @ port, numer portu
        */
        public void AddReceivingSocket(int port)
        {
            UDPSocket socket = new UDPSocket();
            socket.Server(Utils.destinationIP, port, this);
            receivingSockets
                .Add(socket);
        }
        /*
         * Pobiera pakiet od socketu, a nastepnie przesyla go dalej
         * @ packet, tresc pakietu
         */
        public void ReadPacket(string packet)
        {
            _packet = packet;
            //jeezeli FEC != 0, to znaczy, ze jest etykieta mpls
            destinationHost = ReadDestinationHost(_packet);
            ShowMessage(_packet);
            //przesyla pakiet do nastepnego wezla
            SendPacket();
        }
        /*
         * Pomocnicza metoda, wypisuje tresc odebranej wiadomosci 
         * @ message, tresc wiadomosci
         */
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);

        }
        /*
         * Odczytuje z tresci wiadomosci nazwe hosta docelowego i przypisuje go do @ destinationHost
         * @ message, tresc wiadomoci
         */
        public string ReadDestinationHost(string message)
        {
            //wiadomosc przekonwertowana do tablicy bajtow
            byte[] byteMessage = Encoding.ASCII.GetBytes(message);
            //licznik dlugosci nazwy hosta
            int counter = 0;
            //petla liczy na ktorym bajcie wiadomosci jest znak konca nazwy hosta 
            while (counter < byteMessage.Length && byteMessage[counter] != ':')
                counter++;
            //pomocnicza tablica bajtow, do ktorej zapisywana jest nazwa hosta docelowego
            byte[] hostName = new byte[counter];
            for (int i = 0; i < counter; i++)
                hostName[i] = byteMessage[i];

            //Console.WriteLine("Nazwa hosta");
            //Console.WriteLine(destinationHost);
            //Console.WriteLine("/////////////");
            return Encoding.ASCII.GetString(hostName);


        }
        ///*
        // * Odczytuje FEC czyli ID tunelu
        // * @ message, tresc wiadomosci
        // * @ return ID tunelu
        // */
        //public int ReadFECValue(string message)
        //{
        //    //wiadomosc przekonwertowana do tablicy bajtow
        //    byte[] byteMessage = Encoding.ASCII.GetBytes(message);
        //    int counter = 0;
        //    //petla liczy na ktorym bajcie wiadomosci jest znak konca nazwy hosta 
        //    while (counter < byteMessage.Length && byteMessage[counter] != ':')
        //        counter++;
        //    int startIndex = ++counter; //indeks, na ktorym zaczyna sie FEC
        //                                //counter jest na znaku ':' stad inkrementacja
        //    //petla liczy na ktorym bajcie wiadomosci jest znak konca naglowka 
        //    while (counter < byteMessage.Length && byteMessage[counter] != ';')
        //        counter++;
        //    int FEC_Length = counter - startIndex; //dlugosc nr tunelu
        //    byte[] FEC = new byte[FEC_Length];
        //    for (int i = 0; i < FEC_Length; i++)
        //    {
        //        FEC[i] = byteMessage[startIndex];
        //        Console.Write(FEC[i]);
        //        startIndex++;
        //    }
        //    return Int32.Parse(Encoding.ASCII.GetString(FEC));    


        //}
        /*
         * Metoda pomocnicza, do testowania
         * Wysyla pakiet odpowiednim portem, dla danego hosta docelowego
         */
        public void SendPacket(string message, int port)
        {
            for (int i = 0; i < sendingSockets.Count; i++)
                if (sendingSockets[i].getPort() == port)
                {
                    sendingSockets[i].Send(message);
                    return;
                }
            Console.WriteLine("Nie mozna wyslac pakietu zadanym portem");

        }
        /*
         * Na postawie wartosci @ destinationHost wysyla pakiet odpowiednim portem
         */
        public void SendPacket()
        {
            if (CheckMPLSTable())
            {
                //....tutaj bedzie komunikacja z systemem zarzadzania
            }
            else
            {
                int port = CheckIPTable();
                //jezeli metoda zwrocila nr portu 0, to znaczy, ze nie ma takiego portu
                if (port == 0)
                {
                    Console.WriteLine("Nie mozna wyslac pakietu zadanym portem");
                    return;
                }
                //nastepnie szuka socketu o odpowiednim numerze portu i wysyla nim 
                //pobrana przy odbiorze tresc pakietu
                for (int j = 0; j < sendingSockets.Count; j++)
                    if (sendingSockets[j].getPort() == port)
                    {
                        sendingSockets[j].Send(_packet);
                        return;
                    }
                //jezeli nie udalo sie wyslac, zwraca komunikat
                Console.WriteLine("Nie mozna wyslac pakietu zadanym portem");
            }

        }


        /*
         * Metoda sprawdza tablice MPLS-FIB, 
         * zwraca true dla FEC != 0, w przeciwnym wypadku false
         */
        public bool CheckMPLSTable()
        {
            for (int i = 0; i < tableMPLS_FIB.Count; i++)
                if (tableMPLS_FIB[i].GetHostName().Equals(destinationHost))
                {
                    if (tableMPLS_FIB[i].GetFEC() != 0) return true; //jesli FEC != 0 zwraca true
                    else return false;
                }
            return false; //jesli nie bylo zadnego wpisu zwraca false

        }
        /*
         * Metoda sprawdza tablice IP-FIB, 
         * zwraca nr portu, ktorym pakiet ma zostac wyslany
         */
        public int CheckIPTable()
        {
            //pierwszy for szuka odpowiedniego wiersza tablicy routingowej, po nazwie hosta
            //i odczytuje jego nr portu
            for (int i = 0; i < tableIP_FIB.Count; i++)
                if (tableIP_FIB[i].GetHostName().Equals(destinationHost))
                {
                    return tableIP_FIB[i].GetPort();
                }
            return 0; //jezeli nie ma takiego wiersza zwraca 0
        }


    }
}
