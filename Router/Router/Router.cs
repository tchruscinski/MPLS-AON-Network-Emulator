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
        private List<FTNLine> tableFTN = new List<FTNLine>();//tablica FTN
        private List<NHLFELine> tableNHLFE = new List<NHLFELine>(); //tablica NHLFE
        //sockety, ktorymi pakiety sa przesylane dalej
        private List<UDPSocket> sendingSockets = new List<UDPSocket>();
        //sockety, ktore odbieraja pakiety
        private List<UDPSocket> receivingSockets = new List<UDPSocket>();
        private String _packet = " "; //tresc pakietu obslugiwanego w danym momencie przez router,
        private String destinationHost = " "; //docelowy host pakietu obslugiwanego w danym momencie
        private int labelStartIndex; //nr bajtu, na ktorym zaczyna sie etykieta mpls

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
        public void AddFTNLine(FTNLine newLine) { tableFTN.Add(newLine); }
        public void AddNHLFELine(NHLFELine newLine) { tableNHLFE.Add(newLine); }

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
            labelStartIndex = counter;
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
            int port = 0; //nr portu, ktorym wyslemy pakiet
            int FEC = CheckMPLSTable(); //odczytujemy wartosc FEC z tablicy MPLS-FIB
            if (FEC != 0)
            {
                int NHLFE_ID = CheckFTNTable(FEC); //odczytujemy ID wpisu NHLFE z tablicy FTN
                                                   //petla zakomentowana do testow
                                                   //while(NHLFE_ID != 0) //sprawdzamy wpisy NHLFE, az dojdziemy do wpisu o numerze 0
                                                   //{
                int index = CheckNHLFETable(NHLFE_ID); //szukamy indeksu wpisu o danym ID
                if (index == -1) Console.WriteLine("Nie mozna wyslac pakietu zadanym portem");
                else
                {
                    NHLFELine line = tableNHLFE[index];
                    //odczytujemy etykiete
                    int label = line.getLabel();

                    //obsluga wszystkich rodzajow akcji
                    if (line.getAction() == Action.PUSH)
                    {
                        AddLabel(label);
                    }
                    else if (line.getAction() == Action.POP)
                    {
                        //...
                    }
                    else
                    {
                        //...
                    }
                    //odczytujemy nr portu
                    port = line.getPort();
                  
                }
                //}

            }
            else
            {
                port = CheckIPTable(); //odczytujemy nr portu z tablicy IP-FIB
                //jezeli metoda zwrocila nr portu 0, to znaczy, ze nie ma takiego portu
                if (port == 0)
                {
                    Console.WriteLine("Nie mozna wyslac pakietu zadanym portem");
                    return;
                }
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

        


        /*
         * Metoda sprawdza tablice MPLS-FIB, 
         * zwraca true wartosc FEC odczytana z tablicy
         * jesli nie znalazla zadnej wartosci zwraca 0
         */
        public int CheckMPLSTable()
        {
            for (int i = 0; i < tableMPLS_FIB.Count; i++)
                if (tableMPLS_FIB[i].GetHostName().Equals(destinationHost))
                {
                    if (tableMPLS_FIB[i].GetFEC() != 0) return tableMPLS_FIB[i].GetFEC(); 
                    
                }
            return 0;

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
        /*
         * Metoda sprawdza tablice FTN, 
         * zwraca ID wpisu NHLFE
         * @ FEC poszukiwana wartosc FEC
         */
        public int CheckFTNTable(int FEC)
        {
            for (int i = 0; i < tableFTN.Count; i++)
                if (tableFTN[i].GetFEC() == FEC)
                {
                    return tableFTN[i].GetId();
                }
            return 0; //jezeli nie ma takiego wiersza zwraca 0
        }
        /*
         * Metoda sprawdza tablice NHLFE, 
         * szukajac wpisu o odpowiednim ID
         * zwraca jego indeks
         * @ ID, ID poszukiwanego wpisu
         */
        public int CheckNHLFETable(int ID)
        {
            for (int i = 0; i < tableNHLFE.Count; i++)
                if (tableNHLFE[i].getID() == ID)
                {
                    return i;
                }
            return -1; //jezeli nie ma takiego wpisu zwraca -1
        }
        /*
         * Dodaje etykiete do pakietu
         * @ label nr etykiety
         */
        public void AddLabel(int label)
        {
            //tresc pakietu przekonwertowana do tablicy bajtow
            byte[] packet = Encoding.ASCII.GetBytes(_packet);
            //nazwa hosta wraz z znakiem ':'
            //labelStartIndex to dlugosc nazwy, wiec + 1, na znak ':'
            byte[] hostName = new byte[labelStartIndex + 1];
            //reszta wiadomosci
            byte[] message = new byte[packet.Length - labelStartIndex];
            //indeks pomocniczy
            int index = labelStartIndex;
            //labelStartIndex jest na znaku ':', 
            //ktorego nie chcemy drugi raz, wiec stad inkrementacja
            //stad rowniez - 1 w forze
            index++;
            for(int i = 0; i < packet.Length - labelStartIndex - 1; i++)
            {
                message[i] = packet[index];
                index++;
            }
            for (int i = 0; i <= labelStartIndex; i++)
                hostName[i] = packet[i];
            //pakiet jest tworzony od nowa
            // nazwa hosta: -> etykieta -> , -> reszta wiadomosci
            //przecinek do oddzielenia kolejnych etykiet
            StringBuilder newPacket = new StringBuilder(Encoding.ASCII.GetString(hostName));
            newPacket.Append(label);
            newPacket.Append(",");
            newPacket.Append(Encoding.ASCII.GetString(message));

            //nowa tresc pakietu, z dodana etykieta mpls
            _packet = newPacket.ToString();
            Console.WriteLine(_packet);

        }


    }
}
