using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    /*
     * Link resource manager, posiada informacje o łączach połączonych z danym węzłem i ich stanie
     * przekazuje te wiedze do RC
     */
    static class LinkResourceManager
    {
        private static List<Link> links = new List<Link>();
        private static List<RoutingLine> routingLines = new List<RoutingLine>(); //lista portów łącza

        public static List<Link> GetLinks() { return links; }

        /*
         * Metoda dodaje nowe łącze pomiędzy zdefiniowanymi węzłami o określonych parametrach do Link Resource Managera
         *
         * @node1 - nazwa jednego konćowego węzła
         * @node2 - nazwa drugiego konćowego węzła
         * @length - długość łącza
         * @bandWidth - przepustowość łącza
         */
        public static void AddLink(string node1, string node2, int length, double bandWidth)
        {
            links.Add(new Link(node1, node2, length, bandWidth));
        }

        /*
         * Metoda dodaje nowe łącze do Link Resource Managera
         *
         */
        public static void AddLink(Link link)
        {
            links.Add(link);
        }

        /*
     * Metoda dodaje linię routingową do łącza,
     * @ listeningPort - numer portu nasłuchującego
     * @ sendingPort - numer portu wysyłającego
     */
        public static void AddRoutingLine(int listeningPort, int sendingPort)
        {
            routingLines.Add(new RoutingLine(listeningPort, sendingPort));
        }

        /*
      * Metoda zwracająca wszystkie linie routingowe łącza
      */
        public static List<RoutingLine> GetRoutingLines()
        {
            return routingLines;
        }

        public static List<int> GetSendingPorts()
        {
            List<int> sendingPorts = new List<int>();
            for (int i = 0; i < routingLines.Count; i++)
                sendingPorts.Add(routingLines[i].GetSendingPort());

            return sendingPorts;
        }

        public static List<UDPSocket> GetSendingSockets()
        {
            List<UDPSocket> sendingSockets = new List<UDPSocket>();
            for (int i = 0; i < routingLines.Count; i++)
                sendingSockets.Add(routingLines[i].GetSendingSocket());
            return sendingSockets;
        }

        public static List<UDPSocket> GetListeningSockets()
        {
            List<UDPSocket> listeningSockets = new List<UDPSocket>();
            for (int i = 0; i < routingLines.Count; i++)
                listeningSockets.Add(routingLines[i].GetListeningSocket());
            return listeningSockets;
        }

        /*
         * Metoda tworzy Serwery/Klienty
         */
        public static void RunSockets(Node node)
        {
            for(int i = 0; i < routingLines.Count; i++)
            {
                routingLines[i].RunSocket(node);
            }
        }
    }
}
