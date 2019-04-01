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
        private List<RoutingLine> routingTable = new List<RoutingLine>();
        private List<UDPSocket> sockets = new List<UDPSocket>();

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
    }
}
