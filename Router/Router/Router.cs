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

    }
}

