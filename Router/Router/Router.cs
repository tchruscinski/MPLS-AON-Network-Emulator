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
        private List<RoutingLine> routingTable = new List<RoutingLine>(); //FIB(?)
        private List<UDPSocket> sockets = new List<UDPSocket>();
        private String _packet = " "; //tresc pakietu obslugiwanego w danym momencie przez router,
        private String destinationHost = " "; //docelowy host pakietu obslugiwanego w danym momencie

        /*
         * Metoda dodaje 
         * @ newLine, nowy wiersz do tablicy routingowej 
         */
        public void AddRoutingLine(RoutingLine newLine)
        {
            routingTable.Add(newLine);
        }
        /*
         * Metoda dodaje,
         * @ newSocket, nowy socket do listy
         */
        public void AddSocket(UDPSocket newSocket)
        {
            sockets.Add(newSocket);
        }
        /*
         * Pobiera pakiet od socketu
         * @ packet, tresc pakietu
         */
        public void GetPacket(string packet)
        {
            _packet = packet;
            GetDestinationHost(_packet);
            ShowMessage(_packet);
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
        public void GetDestinationHost(string message)
        {
            //wiadomosc przekonwertowana do tablicy bajtow
            byte[] byteMessage = Encoding.ASCII.GetBytes(message);
            //licznik dlugosci nazwy hosta
            int counter = 0;
            //petla liczy na ktorym bajcie wiadomosci jest znak konca naglowka
            while (byteMessage[counter] != ';' && counter < byteMessage.Length)
                counter++;
            //pomocnicza tablica bajtow, do ktorej zapisywana jest nazwa hosta docelowego
            byte[] hostName = new byte[counter];
            for (int i = 0; i < counter; i++)
                hostName[i] = byteMessage[i];

            //Console.WriteLine("Nazwa hosta");
            //Console.WriteLine(destinationHost);
            //Console.WriteLine("/////////////");
            destinationHost = Encoding.ASCII.GetString(hostName);


        }


    }
}
