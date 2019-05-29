using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Node
{
    /*
     * Klasa reprezentuje wezel kliencki, odbiera pakiet, 
     * sprawdza w tablicy routingowej, gdzie kierowac go dalej, po czym wysyla go odpowiednim portem
     * 
     * TODO: Etykiety MPLS
     */
    public class Node
    {
        private string name = ""; //nazwa routera
        //sockety, ktorymi pakiety sa przesylane dalej
        private List<UDPSocket> sendingSockets = new List<UDPSocket>();
        //sockety, ktore odbieraja pakiety
        private List<UDPSocket> receivingSockets = new List<UDPSocket>();
        private String destinationHost = " "; //docelowy host pakietu obslugiwanego w danym momencie
        private int labelStartIndex; //nr bajtu, na ktorym zaczyna sie etykieta mpls
        private int _incPort; //port, ktorym przyszedl pakiet, potrzebny do tablicy ILM
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
            RoutingController.SetNode(this);
            CallController.InitiateCC(this);
            Thread.Sleep(2000); //pauza, żeby wszystkie węzły zdążyły się włączyć
            SendHello();
            Thread.Sleep(1000);
            RoutingController.SetInitialLinks(LinkResourceManager.GetLinks()); //LRM przekazuje RC informacje o stanie łączy
            LinkResourceManager.ShowLinks();
         

        }
        public string GetName() { return this.name; }
        public void SetIncPort(int incPort) { _incPort = incPort; } 
        public List<UDPSocket> GetSendingSockets() { return sendingSockets; }
        public List<UDPSocket> GetReceivingSockets() { return receivingSockets; }
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
        public void ReadPacket(string packet)
        {
            ShowMessage(packet);
            if (!packet.Equals("ACK"))
            {
                SendFeedback();//wysłanie potwierdzenia otrzymania wiadomości
                return;
            }
            //*****************
            //NA POTRZEBY TESTÓW
            //*****************
            return;

            int port = DetermineSendingPort(packet); // nr portu, ktorym pakiet zostanie wyslany
            if(port == 0) //jezeli RefactorPacket() zwraca 0, to znaczy, ze nie ma takiego portu albo jest jakis blad
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Nie mozna wyslac pakietu zadanym portem/");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }
            //przesyla pakiet do nastepnego wezla
            SendPacket(packet, port);
        }
        /*
         * Wysyła informacje zwrotną po odebraniu wiadomości, domyślnie przesyła ACK
         */
        public void SendFeedback(string message="ACK")
        {
            int index = 0; //szukany index wpisu tablicy routingowej
            foreach(RoutingLine i in LinkResourceManager.GetRoutingLines())
            {
                if (_incPort == i.GetListeneningPort())
                {
                    i.GetSendingSocket().Send(message);
                    return;
                }
                Console.WriteLine("Nie można odesłać wiadomości zwrotnej: " + message);
            }
        }
        /*
         * Węzeł po włączeniu wysyła do wszystkich sąsiadów wiadomość hello
         */
        public void SendHello()
        {
            foreach (RoutingLine i in LinkResourceManager.GetRoutingLines())
                i.GetSendingSocket().Send("Hello; " + name);
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

            Console.WriteLine("Nazwa hosta: " + destinationHost);
            return Encoding.ASCII.GetString(hostName);
        }

        /*
         * Metoda pomocnicza, do testowania
         * Wysyla pakiet odpowiednim portem, dla danego hosta docelowego
         */
        public void SendPacket(string message, int port)
        {
            try
            {
                for (int i = 0; i < sendingSockets.Count; i++)
                    if (sendingSockets[i].getPort() == port)
                    {
                        sendingSockets[i].Send(message);
                        return;
                    }
            } catch (Exception e)
            {
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Nie mozna wyslac pakietu zadanym portem");
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " PORT:" + port);
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + "Błąd: " + e.Message);
            }
        }

        /*
         * Metoda sprawdza odpowiednie wpisy tablic routera i przetwarza jego naglowek
         * @packet - string, pakiet odebrany przez węzeł
         */
        public int DetermineSendingPort(string packet)
        {
            if (packet.Equals("")) return 0;
            string destinationHost = ReadDestinationHost(packet);
            int port = LinkResourceManager.GetSendingPortForDestination(destinationHost);
            return port;
        }


        /*
        * Metoda parsuje konfigurację węzła
        */
        public void ParseNodeConfig()
        {
            try
            {
                List<string> localConfig = parser.ParseConfig(this.name + ".xml");

                //Console.WriteLine("sparsowany xml: "+ localConfig);

                String[] splitConfig = localConfig[0].Split(',');
                int numberOfRows = Int32.Parse(localConfig[1]);
                Console.WriteLine("ilosc rzedow" + numberOfRows);
                //Console.ReadLine();

                if (!splitConfig.Contains("0") && (splitConfig.Contains(null) || splitConfig.Contains("")))
                {
                    Console.WriteLine("Plik konfiguracyjny zawiera puste wartości, nie można wczytać wartości.");
                    return;
                }

                int n = 0;
                int configIterator = 0;
                  
                while (n < numberOfRows)
                {
                    Link link = new Link(
                        this.name,
                        splitConfig[configIterator],
                        Int32.Parse(splitConfig[configIterator + 1]),
                        Int32.Parse(splitConfig[configIterator + 2]),
                        Int32.Parse(splitConfig[configIterator + 3]),
                        Int32.Parse(splitConfig[configIterator + 4])
                    );
                    LinkResourceManager.AddRoutingLine(
                        Int32.Parse(splitConfig[configIterator + 3]),
                        Int32.Parse(splitConfig[configIterator + 4])
                    );
                    //jeżeli, któraś z wartości jest 0, to znaczy, że jest to połączenie z hostem, więc nie dodajemy go do LRM
                    if(Int32.Parse(splitConfig[configIterator + 1]) != 0)
                         LinkResourceManager.AddLink(link);
                    configIterator += 5;
                    n++;
                }
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Lokalna konfiguracja wczytana do węzła " + this.name);
            } catch (NullReferenceException e)
            {
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Nie mozna wczytac pliku konfiguracyjnego");
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + " Błąd: " + e.Message);
            }
        }
    }
}
