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
        private List<NHLFELine> tableNHLFE = new List<NHLFELine>(); //tablica NHLFE
        private List<ILMLine> tableILM = new List<ILMLine>(); //tablica ILM
        //sockety, ktorymi pakiety sa przesylane dalej
        private List<UDPSocket> sendingSockets = new List<UDPSocket>();
        //sockety, ktore odbieraja pakiety
        private List<UDPSocket> receivingSockets = new List<UDPSocket>();
        //sockety do komunikacji z systemem zarzadzania
        private UDPSocket sendingManagementSocket = new UDPSocket();
        private UDPSocket receivingManagementSocket = new UDPSocket();
        private String _packet = " "; //tresc pakietu obslugiwanego w danym momencie przez router,
        private String destinationHost = " "; //docelowy host pakietu obslugiwanego w danym momencie
        private int labelStartIndex; //nr bajtu, na ktorym zaczyna sie etykieta mpls
        private int _incPort; //port, ktorym przyszedl pakiet, potrzebny do tablicy ILM
        private int _topLabel; //etykieta na szczycie stosu etykiet pakietu
        private string _poppedLabels; //etykiety ze stosu etykiet pakietu, poza szczytowa etykieta

        public Router(string name)
        {
            _name = name;
        }
        public string GetName() { return _name; }
        public void SetIncPort(int incPort) { _incPort = incPort; } 

        public void SetSendingManagementSocket(int port)
        {
            sendingManagementSocket.Client(Utils.destinationIP, port, this);
        }
        public void SetReceivingManagementSocket(int port)
        {
            sendingManagementSocket.Server(Utils.destinationIP, port, this);
        }
        public void AddNHLFELine(NHLFELine newLine) { tableNHLFE.Add(newLine); }
        public void AddILMLine(ILMLine newLine) { tableILM.Add(newLine); }

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
        public void AddSendingSocket(UDPSocket newSocket)
        {
            sendingSockets.Add(newSocket);
        }
        public void DeleteNHLFELine(int ID)
        {
            for (int i = 0; i < tableNHLFE.Count; i++)
                if (tableNHLFE[i].getID() == ID) tableNHLFE.Remove(tableNHLFE[i]);
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
            //destinationHost = ReadDestinationHost(_packet);
            ShowMessage(_packet);
            int port = RefactorPacket(); // nr portu, ktorym pakiet zostanie wyslany
            if(port == 0) //jezeli RefactorPacket() zwraca 0, to znaczy, ze nie ma takiego portu albo jest jakis blad
            {
                Console.WriteLine("Nie mozna wyslac pakietu zadanym portem/");
                return;
            }
            //przesyla pakiet do nastepnego wezla
            SendPacket(port);
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
            Console.WriteLine("Nie mozna wyslac pakietu zadanym portem//");

        }
        /*
         * Metoda sprawdza odpowiednie wpisy tablic routera i przetwarza jego naglowek
         * pozostawia pakiet w postaci gotowej do wyslania
         * zwraca nr portu, ktorym pakiet zostanie wyslany
         */
        public int RefactorPacket()
        {
            int port = 0; //nr portu, ktorym wyslemy pakiet
            GetLabel(); //pobiera etykiety pakietu
            int NHLFE_ID; //ID wpisu tablicy NHLFE
            if (IsLabel()) //jezeli jest etykieta sprawdzamy tablice ILM
            {
                NHLFE_ID = CheckILMTable();
            }
            else //w innym wypadku nie mozna wyslac pakietu
            {
                return 0;
            }
                
                    while(NHLFE_ID != 0) //sprawdzamy wpisy NHLFE, az dojdziemy do wpisu o numerze 0
                    {
                    //jezeli akcja byl pop, schemat postepowania jest nieco inny, 
                    //wartosc nextID wiersza NHLFE nie jest sprawdzana
                    bool wasPop = false; 
                    int index = CheckNHLFETable(NHLFE_ID); //szukamy indeksu wpisu o danym ID
                    if (index == -1)
                        return 0;
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
                            PopLabel();
                            GetTopLabel(); //pobiera etykiety pakietu
                            NHLFE_ID = CheckILMTable(); //po zdjeciu etykiety ponownie sprawdzamy tablice ILM
                         wasPop = true;
                         Console.WriteLine("NHLFE " + NHLFE_ID);
                        }
                        else
                        {
                            SwapLabel(label);
                        }
                        //odczytujemy nr portu
                        port = line.getPort();
                       if(!wasPop)
                          NHLFE_ID = line.getNextID();
                    }   
                    
                    }
               return port;
            
        }
        /*
         * Metoda wysyla pakiet podanym portem
         * @ port, nr portu, ktorym pakiet zostanie wyslany
         */
        public void SendPacket(int port)
        {
            
                //nastepnie szuka socketu o odpowiednim numerze portu i wysyla nim 
                //pobrana przy odbiorze tresc pakietu
                for (int j = 0; j < sendingSockets.Count; j++)
                    if (sendingSockets[j].getPort() == port)
                    {
                        sendingSockets[j].Send(_packet);
                        return;
                    }
                //jezeli nie udalo sie wyslac, zwraca komunikat
                Console.WriteLine("Nie mozna wyslac pakietu zadanym portem///");
        }
        /*
         * Sprawdza tablice NHLFE i zwraca indeks wpisu o żądanym ID
         * @ ID, ID wpisu NHLFE
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
         * Sprawdza tablice ILM 
         * @ 
         */
        public int CheckILMTable()
        {
            for(int i = 0; i < tableILM.Count; i++)
            {
                //Console.WriteLine(_incPort);
                //Console.WriteLine(_topLabel);
                //Console.WriteLine(tableILM[i].GetPort());
                //Console.WriteLine(tableILM[i].GetLabel());
                //Console.WriteLine("Popped labels")
                //Console.WriteLine(_poppedLabels);
                //Console.WriteLine(tableILM[i].GetPoppedLabels());

                if (tableILM[i].GetPort() == _incPort && tableILM[i].GetLabel() == _topLabel
                    && tableILM[i].GetPoppedLabels().Equals(_poppedLabels))
                {
                    //Console.WriteLine(tableILM[i].GetNHLFE());
                    return tableILM[i].GetNHLFE(); //zwraca NHLFE danego wpisu
                }
            }
            return 0; //jesli nie znaleziono 0
            
        }
        /*
         * Dodaje etykiete do pakietu
         * @ label nr etykiety
         */
        public void AddLabel(int label)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(label);
            builder.Append(',');
            builder.Append(_packet);
            _packet = builder.ToString();
        }
        /*
         * Zamienia etykiete na szczycie stosu etykiet pakietu
         * @ label nr etykiety
         */
        public void SwapLabel(int label)
        {
            String[] extractTopLabel = _packet.Split(','); //wydzielamy etykiete ze szczytu stosu, zeby ja podmienic
            StringBuilder builder = new StringBuilder();
            builder.Append(label);
            builder.Append(',');
            for (int i = 1; i < extractTopLabel.Length; i++) //dodajemy reszte pakietu
                builder.Append(extractTopLabel[i]);
            _packet = builder.ToString();
        }
        /*
         * Zdejmuje etykiete ze szczytu stosu etykiet
         * @ label nr etykiety
         */
        public void PopLabel()
        {
            String[] extractTopLabel = _packet.Split(','); //wydzielamy etykiete ze szczytu stosu, zeby ja zdjac
            StringBuilder poppedLabelBuilder = new StringBuilder(); //aktualizujemy wartosc poppedLabels pakietu
            poppedLabelBuilder.Append(extractTopLabel[0]);
            poppedLabelBuilder.Append("-");
            poppedLabelBuilder.Append(_poppedLabels); //dopisujemy reszte zdjetych etykiet
            poppedLabelBuilder.Length--; //usuwamy '-' na ostatnim polu
            _poppedLabels = poppedLabelBuilder.ToString();

            StringBuilder messageBuilder = new StringBuilder();
            for (int i = 1; i < extractTopLabel.Length; i++) //dodajemy reszte pakietu
                messageBuilder.Append(extractTopLabel[i]);

            _packet = messageBuilder.ToString();
            Console.WriteLine("PAkiet: "+ _packet);
        }
        /*
         * Sprawdza czy przychodzacy pakiet ma etykiete MPLS
         * jesli tak, zwraca true
         */
        public bool IsLabel()
        {
            //tresc pakietu przekonwertowana do tablicy bajtow
            byte[] packet = Encoding.ASCII.GetBytes(_packet);
            for (int i = 0; i < packet.Length; i++)
                if (packet[i] == ':')
                    if (packet[i + 1] == ';') //jezeli ';' jest jeden bajt po ':' to znaczy, ze nie ma etykiety
                        return false;
                    else // w innym wypadku jest
                        return true;
            return true; //raczej nigdy nie powinno tutaj dojsc, jesli pakiet jest dobrze zrobiony
        }
        /*
         * Pobiera etykiety pakietu
         * 
         */
        public void GetLabel()
        {
            String[] extractLabelsPart = _packet.Split(':'); //pakiet jest podzielony na czesc etykiet i reszte
            String[] extractLabels = extractLabelsPart[0].Split(','); //reszta naglowka podzielona na etykiety
            Console.WriteLine("Etykiety:");
            if (extractLabels[0].Length != 0)
            {
                for (int i = 0; i < extractLabels.Length; i++)
                {
                    Console.WriteLine(extractLabels[i]);
                }
                _topLabel = Int32.Parse(extractLabels[0]); //pierwsza etykieta zapisana jako etykieta ze szczytu
                StringBuilder builder = new StringBuilder();
                for (int i = 2; i < extractLabels.Length - 1; i++) //pozostale etykiety dodane po myslniku
                {
                    builder.Append(extractLabels[i]);
                    builder.Append('-');
                }
                _poppedLabels = builder.ToString();

            }


        }
        /*
         * Pobiera tylko etykiete ze szczytu
         * 
         */
        public void GetTopLabel()
        {
            String[] extractLabelsPart = _packet.Split(';'); //pakiet jest podzielony na czesc etykiet i reszte
            String[] extractLabels = extractLabelsPart[0].Split(','); //reszta naglowka podzielona na etykiety
            Console.WriteLine("Etykiety:");
            if (extractLabels[0].Length != 0)
            {
                for (int i = 0; i < extractLabels.Length; i++)
                {
                    Console.WriteLine(extractLabels[i]);
                }
                _topLabel = Int32.Parse(extractLabels[0]); //pierwsza etykieta zapisana jako etykieta ze szczytu
                Console.WriteLine("top" + _topLabel);
            }


        }
        /*
         * Wysyla zadanie tablic NHLFE i ILM do systemu zarzadzania,
         * 
         */
        public void ManagementRequest()
        {
            sendingManagementSocket.Send(_name);
        }






        /*
 * Metoda sprawdza tablice FTN, 
 * zwraca ID wpisu NHLFE
 * @ FEC poszukiwana wartosc FEC
 */
        //public int CheckFTNTable(int FEC)
        //{
        //    for (int i = 0; i < tableFTN.Count; i++)
        //        if (tableFTN[i].GetFEC() == FEC)
        //        {
        //            return tableFTN[i].GetId();
        //        }
        //    return 0; //jezeli nie ma takiego wiersza zwraca 0
        //}
        /*
         * Metoda sprawdza tablice NHLFE, 
         * szukajac wpisu o odpowiednim ID
         * zwraca jego indeks
         * @ ID, ID poszukiwanego wpisu
         */
    }
}
