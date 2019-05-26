using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    /*
     * Klasa reprezentuje wezel kliencki, odbiera pakiet, 
     * sprawdza w tablicy routingowej, gdzie kierowac go dalej, po czym wysyla go odpowiednim portem
     * 
     * TODO: Etykiety MPLS
     */
    class Node
    {
        private string name = " "; //nazwa routera
        //sockety, ktorymi pakiety sa przesylane dalej
        private List<UDPSocket> sendingSockets = new List<UDPSocket>();
        //sockety, ktore odbieraja pakiety
        private List<UDPSocket> receivingSockets = new List<UDPSocket>();
        private String _packet = " "; //tresc pakietu obslugiwanego w danym momencie przez router,
        private String destinationHost = " "; //docelowy host pakietu obslugiwanego w danym momencie
        private int labelStartIndex; //nr bajtu, na ktorym zaczyna sie etykieta mpls
        private int _incPort; //port, ktorym przyszedl pakiet, potrzebny do tablicy ILM
        private int _topLabel; //etykieta na szczycie stosu etykiet pakietu
        //etykiety ze stosu etykiet pakietu, poza szczytowa etykieta
        //robie tak dziwnie, bo dla innych przypisan pustego stringa cos sie wywala potem
        private string _poppedLabels = new StringBuilder().ToString(); 
        private static Parser parser = new Parser();
        public static string destinationIP = "127.0.0.1"; //docelowe ip
        static Time time = new Time();
        private String timeStamp = time.GetTimestamp(DateTime.Now);

        public Node(string name)
        {
            this.name = name;
            ParseNodeConfig();
            LinkResourceManager.RunSockets(this); //sockety zaczynaja nasluchiwac/być gotowe do wysłania
            sendingSockets = LinkResourceManager.GetSendingSockets();
            receivingSockets = LinkResourceManager.GetListeningSockets();

        }
        public string GetName() { return this.name; }
        public void SetIncPort(int incPort) { _incPort = incPort; } 

        public string GetDestinationHost() { return destinationHost; }
        /*
         * Metoda dodaje,
         * @ newSocket, nowy socket do listy
         */
        public void AddReceivingSocket(UDPSocket newSocket)
        {
            receivingSockets.Add(newSocket);
        }
        public void AddSendingSocket(UDPSocket newSocket)
        {
            sendingSockets.Add(newSocket);
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
        /*public void ReadPacket(string packet)
        {
            //to do poprawy
            String[] extractedHead = packet.Split(';');
            if(extractedHead[0].Equals("NMS"))
            {
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Otrzymano pakiet od NMS...");
                ParseNMSResponse(packet);
                return;
            }
            _packet = packet;
            //destinationHost = ReadDestinationHost(_packet);
            ShowMessage(_packet);
            int port = RefactorPacket(); // nr portu, ktorym pakiet zostanie wyslany
            if(port == 0) //jezeli RefactorPacket() zwraca 0, to znaczy, ze nie ma takiego portu albo jest jakis blad
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Nie mozna wyslac pakietu zadanym portem/");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }
            //przesyla pakiet do nastepnego wezla
            SendPacket(port);
        }*/

        /*
         * Pomocnicza metoda, wypisuje tresc odebranej wiadomosci 
         * @ message, tresc wiadomosci
         */
        public void ShowMessage(string message)
        {
            //Console.WriteLine(message);

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
                    _poppedLabels = new StringBuilder().ToString();
                    return;
                }
            _poppedLabels = new StringBuilder().ToString();
            Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Nie mozna wyslac pakietu zadanym portem//");
            //usuniecie wpisu z tablicy
            Console.WriteLine(time.GetTimestamp(DateTime.Now) + " PORT:" + port);
            //ActualizeNHFLETable(port);
            //ShowNHLFETable();

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
                        _poppedLabels = new StringBuilder().ToString(); //po wyslaniu czyscimy bufor zdjetych etykiet
                        return;
                    }
                //jezeli nie udalo sie wyslac, zwraca komunikat
                Console.WriteLine("Nie mozna wyslac pakietu zadanym portem///");
                _poppedLabels = new StringBuilder().ToString(); //czyscimy bufor zdjetych etykiet
    
        }

        public void ParseNodeConfig()
        {
            try
            {
                List<string> localConfig = parser.ParseConfig(this.name + ".xml");

                //Console.WriteLine("sparsowany xml: "+ localConfig);

                String[] splitConfig = localConfig[0].Split(',');
                int numberOfRows = Int32.Parse(localConfig[1]);
                Console.WriteLine("ilosc rzedow" + numberOfRows);
                Console.ReadLine();

                if (splitConfig.Contains(null) || splitConfig.Contains(""))
                {
                    Console.WriteLine("Plik konfiguracyjny zawiera puste wartości, nie można wczytać wartości.");
                    return;
                }

                int n = 0;
                int configIterator = 0;
                  
                while (n < numberOfRows)
                {
                    int numberOfRoutingLines = Int32.Parse(splitConfig[configIterator + 3]);
                    Link link = new Link(
                        this.name,
                        splitConfig[configIterator],
                        Int32.Parse(splitConfig[configIterator + 1]),
                        Int32.Parse(splitConfig[configIterator + 2])
                    );

                    for(int i = 0; i < numberOfRoutingLines; i++)
                    {
                        LinkResourceManager.AddRoutingLine(
                            Int32.Parse(splitConfig[configIterator  + 4 + i * 2]),
                            Int32.Parse(splitConfig[configIterator  + 5 + i * 2])
                        );
                    }
                    LinkResourceManager.AddLink(link);
                    configIterator +=  numberOfRoutingLines * 2 + 4;
                    n++;
                }
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Lokalna konfiguracja wczytana do routera " + this.name);
            } catch (NullReferenceException e)
            {
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Nie mozna wczytac pliku konfiguracyjnego");
            }
        }
    }
}
