using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    /*
     * Klasa reprezentuje łącze miedzy węzłami
     */
    public class Link
    {
        //WAZNE:
        // częstotliwości podajemy domyślnie w GHz, żeby nie mieć takich wielkich liczb
        // jak gdzieś częstotliwość jest równa 10, to znaczy, że jest 10 GHz
        private Tuple<string, string> _nodes; //nazwy węzłów połączone danym łączem
        private RoutingLine routingLine;
        private int _length; //dlugosc lacza w kilometrach
        private double _bandWidth; //przepustowość łącza
        private int _slotNumber; //ilość slotów
        private double _slotWidth = 12.5; // szerokosc slotu, w eonie 12,5 Ghz

        //lista zawiera indeksy slotów oraz informacje o tym czy dany slot jest w tym momencie zajęty
        //jest to po to potrzebne, że na całej długości połączenia sygnał musi iść dokładnie tymi samymi
        //szczelinami, tj. o tych samych indeksach, więc przed zestawieniem połączenia musimy je zarezerwować
        //TRUE = wolny link, FALSE = zajęty link
        private Dictionary<int, bool> _slotIndexList = new Dictionary<int, bool>();

        public Link(string node1, string node2, int length, double bandWidth, int listeningPort, int sendingPort)
        {
            _nodes = new Tuple<string, string>(node1, node2);
            _length = length;
            _bandWidth = bandWidth;
            _slotNumber = (int)Math.Round(_bandWidth / _slotWidth);
            //Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", _nodes.Item1, _nodes.Item2,
            //    _length, _bandWidth, _slotNumber, _slotWidth);
            for(int i = 0; i < _slotNumber; i++)
            {
                _slotIndexList.Add(i, true); //na początku wszystkie są wolne
                //Console.WriteLine("Slot o numerze: {0}, status: {1}",
                //    _slotIndexList.Keys.ElementAt(i), _slotIndexList.Values.ElementAt(i));
            }

            routingLine = new RoutingLine(listeningPort, sendingPort);
         
        }

        public RoutingLine GetRoutingLine()
        {
            return routingLine;
        }

        public Tuple<string, string> GetConnectedNodes()
        {
            return _nodes;
        }
        ///*
        //* Metoda dodaje linię routingową do łącza,
        //* @ listeningPort - numer portu nasłuchującego
        //* @ sendingPort - numer portu wysyłającego
        //*/
        //public void addRoutingLine(int listeningPort, int sendingPort)
        //{
        //    routingLines.Add(new RoutingLine(listeningPort, sendingPort));
        //}

        ///*
        //* Metoda zwracająca wszystkie linie routingowe łącza
        //*/
        //public List<RoutingLine> getRoutingLines()
        //{
        //    return routingLines;
        //}
    }
}
