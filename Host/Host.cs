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
        private int id; //id hosta
        private string _name = " "; //nazwa hosta
        private UDPSocket sendingSocket = new UDPSocket(); //socket, ktory wysyla pakiety
        private UDPSocket receivingSocket = new UDPSocket(); //sokcet, ktory nasluchuje na przychodzace pakiety
        private int _sendingPort; // nr portu, ktorym sendingSocket wysyla pakiety
        private int _receivingPort; //nr portu, na ktorym receivingPort nasluchuje
        private List<NHLFELine> tableNHLFE = new List<NHLFELine>(); //tablica NHLFE
        private List<MPLSLine> tableMPLS_FIB = new List<MPLSLine>(); //tablica routingowa MPLS
        private List<ILMLine> tableILM = new List<ILMLine>(); //tablica ILM
        public static string destinationIP = "127.0.0.1"; //docelowe ip
        static Time time = new Time();
        private static Parser parser = new Parser();
        private String timeStamp = time.GetTimestamp(DateTime.Now);

        public void AddNHLFELine(NHLFELine newLine) { tableNHLFE.Add(newLine); }
        public void AddRoutingLineMPLS(MPLSLine newLine) { tableMPLS_FIB.Add(newLine); }
        public void AddILMLIne(ILMLine newLine) { tableILM.Add(newLine); }
        public Host(string name)
        {
            _name = name;
            ParseLocalConfig();
            ParseHostTable();
        }

        public Host(string name,int sendingPort, int receivingPort)
        {
            _name = name;
            _sendingPort = sendingPort;
            _receivingPort = receivingPort;
            sendingSocket.Client(Utils.destinationIP, _sendingPort, this);
            receivingSocket.Server(Utils.destinationIP, _receivingPort, this);
            ParseLocalConfig();
            ParseHostTable();
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

            String[] extractedLabel = packet.Split(','); //wydzielamy czesc etykiet, bo jej nie potrzebujemy

            String sender = GetSenderName(extractedLabel[0]);//nazwa nadawcy
            String[] extractedSender = packet.Split(';'); //wydzielamy nadawce i tresc wiadomosci
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(time.GetTimestamp(DateTime.Now) + _name + " od: " + sender +  " otrzymal:" + extractedSender[1]);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /*
         * Metoda parsująca responsa od NMS-a z konfiguracją(tabelą) dla hosta
         * @ response - string, reponse od NMS-a
         */
         public void ParseHostTable()
         {
            string table = parser.ParseHostTable("host_config.xml", _name);
            String[] tableContent = table.Split(',');

            while(tableContent.Length >= 7)
            {
                string DestinationHost = tableContent[0];
                int NHLFE_ID = (tableContent[1] == null || tableContent[1].Equals("")) ? 0 : Int32.Parse(tableContent[1]);
                tableMPLS_FIB.Add(new MPLSLine(DestinationHost, NHLFE_ID));

                int Label = (tableContent[2] == null || tableContent[2].Equals("")) ? 0 : Int32.Parse(tableContent[2]);
                string Sender = tableContent[3];
                tableILM.Add(new ILMLine(Label, Sender));

                int ID = (tableContent[4] == null || tableContent[4].Equals("")) ? 0 : Int32.Parse(tableContent[4]);
                int NLabel = (tableContent[5] == null || tableContent[5].Equals("")) ? 0 : Int32.Parse(tableContent[5]);
                int NNextIdLabel = (tableContent[6] == null || tableContent[6].Equals("")) ? 0 : Int32.Parse(tableContent[6]);
                tableNHLFE.Add(new NHLFELine(ID, NLabel, NNextIdLabel));

                List<string> list = new List<string>(tableContent);
                if(list.Count == 7)
                {
                    break;
                }
                list.RemoveRange(1, 7);
                tableContent = list.ToArray();
            }
         }

        /*
         * Wysyla pakiet danych, dodajac w naglowku nazwe hosta docelowego
         * @ destinationHost, nazwa hosta docelowego, @ message, tresc wiadomosci
         * format wiadomosci etykieta1,etykieta2...:nazwa nadawcy; wiadomosc
         */
        public void SendPacket(string destinationHost, string message)
        {
            StringBuilder builder = new StringBuilder();
            int NHLFE_ID = CheckMPLSTable(destinationHost);
            if (NHLFE_ID == 0)
            {
                Console.WriteLine("Nie mozna wyslac pakietu zadanym portem");
                return;
            }
            else
            {
                while (NHLFE_ID != 0)
                {
                    int index = CheckNHLFETable(NHLFE_ID); //szukamy indeksu wpisu o danym ID
                    if (index == -1) { 
                        Console.WriteLine ("Nie mozna wyslac pakietu zadanym portem");
                        return;
                        }
                    else
                    {
                        NHLFELine line = tableNHLFE[index];
                        //odczytujemy etykiete
                        int label = line.getLabel();
                        builder.Append(label);
                        builder.Append(','); //przecinek oddziela etykiety
                        NHLFE_ID = line.getNextID();
                    }

                }
                builder.Append(';'); //';' oddziela naglowek od wiadomosci
                builder.Append(message);
                sendingSocket.Send(builder.ToString());
            }
                
        }   

        public string getName()
        {
            return _name;
        }
        /*
         * Metoda sprawdza tablice MPLS-FIB, 
         * zwraca wartosc NHLFE
         * jesli nie znalazla zadnej wartosci zwraca 0
         */
        public int CheckMPLSTable(string destinationHost)
        {
            for (int i = 0; i < tableMPLS_FIB.Count; i++)
                if (tableMPLS_FIB[i].GetHostName().Equals(destinationHost))
                {
                    return tableMPLS_FIB[i].GetNHLFE();

                }
            return 0;

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
         * zwraca nazwe nadawcy
         */
        public string GetSenderName(string label)
        {
            for (int i = 0; i < tableILM.Count; i++)
            {
                if ((tableILM[i].GetLabel().ToString()).Equals(label))
                {
                    return tableILM[i].GetSender();
                }
            }
            return ""; //jesli nie ma takiego wpisu w tabeli zwraca pustego stringa
        }

        public void ParseLocalConfig()
        {
            try
            {
                string localConfig = parser.ParseLocalConfig(_name + ".xml");
                Console.WriteLine("Zparsowany local config xml: "+ localConfig);

                String[] splitConfig = localConfig.Split(',');
                if (splitConfig.Contains(null) || splitConfig.Contains(""))
                {
                    return;
                }
                sendingSocket.Client(Utils.destinationIP, Int32.Parse(splitConfig[1]), this);
                receivingSocket.Server(Utils.destinationIP, Int32.Parse(splitConfig[3]), this);

                Console.WriteLine("Lokalna konfiguracja wczytana do hosta " + _name);
            } catch(NullReferenceException e)
            {
                Console.WriteLine("Nie mozna wczytac pliku konfiguracyjnego");
            }
        }
        /*
         * Wyswietla na ekran konsoli wartosci wszystkich wierszy tablic routingowych
         */
        public void ShowRoutingLines()
        {
            Console.WriteLine("ILMTable:");
            for (int i = 0; i < tableILM.Count; i++)
                tableILM[i].ShowILMLine();
            Console.WriteLine("NHLFETable:");
            for (int i = 0; i < tableNHLFE.Count; i++)
                tableNHLFE[i].ShowNHLFELine();
            Console.WriteLine("MPLSTable:");
            for (int i = 0; i < tableMPLS_FIB.Count; i++)
                tableMPLS_FIB[i].ShowMPLSLine();

        }
    }
}