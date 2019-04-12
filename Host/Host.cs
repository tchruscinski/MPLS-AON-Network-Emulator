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
        private int _sendingPort; // nr portu, ktorym sendingSocket wysyla pakiety
        private UDPSocket receivingSocket = new UDPSocket(); //sokcet, ktory nasluchuje na przychodzace pakiety
        private int _receivingPort; //nr portu, na ktorym receivingPort nasluchuje
        private List<NHLFELine> tableNHLFE = new List<NHLFELine>(); //tablica NHLFE
        private List<MPLSLine> tableMPLS_FIB = new List<MPLSLine>(); //tablica routingowa MPLS
        private List<ILMLine> tableILM = new List<ILMLine>(); //tablica ILM

        public void AddNHLFELine(NHLFELine newLine) { tableNHLFE.Add(newLine); }
        public void AddRoutingLineMPLS(MPLSLine newLine) { tableMPLS_FIB.Add(newLine); }
        public void AddILMLIne(ILMLine newLine) { tableILM.Add(newLine); }
        public Host(string name)
        {
            _name = name;
        }

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

            String[] extractedLabel = packet.Split(','); //wydzielamy czesc etykiet, bo jej nie potrzebujemy
            //nazwa nadawcy
            String sender = GetSenderName(extractedLabel[0]);
            String[] extractedSender = extractedLabel[1].Split(';'); //wydzielamy nadawce i tresc wiadomosci
            Console.WriteLine(_name + " od: " + sender +  " otrzymal:" + extractedSender[1]);
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
                Console.WriteLine(builder.ToString());
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
                    Console.WriteLine(tableMPLS_FIB[i].GetNHLFE());
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
            Console.WriteLine(label);
            Console.WriteLine(tableILM[0]);
            for (int i = 0; i < tableILM.Count; i++)
            {
                if ((tableILM[i].GetLabel().ToString()).Equals(label))
                {
                    return tableILM[i].GetSender();
                }
            }
            return ""; //jesli nie ma takiego wpisu w tabeli zwraca pustego stringa
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