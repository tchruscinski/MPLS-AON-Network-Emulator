using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConnectionCloud
{
    public class ConnectionCloud
    {
        private List<RoutingTableLine> routingTable = new List<RoutingTableLine>(); //FIB(?)
        //sockety, ktorymi pakiety sa przesylane dalej
        private List<UDPSocket> sendingSockets = new List<UDPSocket>();
        //sockety, ktore odbieraja pakiety
        private List<UDPSocket> receivingSockets = new List<UDPSocket>();
        private String _packet = " "; //tresc pakietu obslugiwanego w danym momencie przez chmurę,
        private String destinationHost = " "; //docelowy host pakietu obslugiwanego
        private TextProcessor _textProcessor = new TextProcessor();
        private static CableEmulatorTableParser cetParser = new CableEmulatorTableParser();
        private String[] _processedText;
        Time time = new Time();
        int outPort;

        //TODO - wypelnianie tabeli routingu danymi
        public void Proceed(string packet, int receivingPort)
        {
            _packet = packet;
            Console.WriteLine("OK");
            try
            {
                Console.WriteLine("sending packet");
                SendPacket(packet, receivingPort);
                return;
            }

            catch(Exception e)
            {
                return;
            }
            
        }
        
       
        public void AddRoutingTable(List<RoutingTableLine> rtl_1)
        {
            routingTable = rtl_1;
        }
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

        public void StartSockets()
        {
            for (int i = 0; i < sendingSockets.Count; i++)
            {
                sendingSockets[i].Client("127.0.0.1", routingTable[i]._outgoingPort);
            }
            for (int i = 0; i < receivingSockets.Count; i++)
            {
                receivingSockets[i].Server("127.0.0.1", routingTable[i]._incomingPort, this);
            }
        }
        /*
         * Odczytuje z tresci wiadomosci nazwe hosta docelowego i przypisuje go do @ destinationHost
         * @ message, tresc wiadomoci
         */
        /*
         * Metoda pomocnicza, do testowania
         * Wysyla pakiet odpowiednim portem, dla danego hosta docelowego
         */
        public void SendPacket(string message, int port)
        {
            for (int i = 0; i < sendingSockets.Count; i++)
            {
                if (routingTable[i]._incomingPort == port)
                {
                    outPort = FindSendPort(port);
                    for (int j = 0; j < sendingSockets.Count; j++)
                    {
                        if (sendingSockets[j].getPort() == outPort)
                        {
                            sendingSockets[j].Send(message);
                            Console.WriteLine(time.GetTimestamp(DateTime.Now) + "Message sent over port: {0}", port);
                            return;
                        }
                    }
                }
                else
                { 

                    Console.WriteLine(time.GetTimestamp(DateTime.Now) + "Message wasn't send");
                }

            }                 
            Console.WriteLine("Nie mozna wyslac pakietu zadanym portem");
            //usuniecie wpisu z tablicy
            Console.WriteLine("PORT:" + port);

        }
        /*
         * Metoda sprawdza odpowiednie wpisy tablic routera i przetwarza jego naglowek
         * pozostawia pakiet w postaci gotowej do wyslania
         * zwraca nr portu, ktorym pakiet zostanie wyslany
         */

        /*
         * Metoda wysyla pakiet podanym portem
         * @ port, nr portu, ktorym pakiet zostanie wyslany
         */
        public int FindSendPort(int port)
        {

            //nastepnie szuka socketu o odpowiednim numerze portu i wysyla nim 
            //pobrana przy odbiorze tresc pakietu
            for (int i = 0; i < sendingSockets.Count; i++)
            {
                if (routingTable[i]._incomingPort == port)
                {

                    return routingTable[i]._outgoingPort;
                }
            }
            //jezeli nie udalo sie znalezc wlasciwego portu, zwraca komunikat
            Console.WriteLine("Nie istnieje szukany port wysylajacy");
            return 0;
        }
        



    }
}
