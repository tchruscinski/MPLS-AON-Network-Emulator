using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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
        private double _bandWidth; //szerokość pasma
        private int _slotNumber; //ilość slotów
        private double _capacity; //przepustowość łącza
        private double _slotWidth = 12.5; // szerokosc slotu, w eonie 12,5 Ghz
        private int bandEfficiency = 2; // efektywnosc widmowa, przyjmujemy 2 Hz/Baud

        //lista zawiera indeksy slotów oraz informacje o tym czy dany slot jest w tym momencie zajęty
        //jest to po to potrzebne, że na całej długości połączenia sygnał musi iść dokładnie tymi samymi
        //szczelinami, tj. o tych samych indeksach, więc przed zestawieniem połączenia musimy je zarezerwować
        //TRUE = wolny link, FALSE = zajęty link
        private Dictionary<int, bool> _slotIndexList = new Dictionary<int, bool>();
        public string GetNode1() { return _nodes.Item1; }
        public string GetNode2() { return _nodes.Item2; }
        public int GetLength() { return _length; }
        public double GetCapacity() { return _capacity; }


        public Link(string node1, string node2, int length, double capacity)
        {
            _nodes = new Tuple<string, string>(node1, node2);
            _length = length;
            _capacity = capacity;
            _bandWidth = _capacity * bandEfficiency; //szerokość pasma to przepustowość * efektywność widmowa
            _slotNumber = (int)Math.Floor(_bandWidth / _slotWidth); //Floor() - zaokrąglenie w dół
            //Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", _nodes.Item1, _nodes.Item2,
            //    _length, _bandWidth, _slotNumber, _slotWidth);
            for(int i = 0; i < _slotNumber; i++)
            {
                _slotIndexList.Add(i, true); //na początku wszystkie są wolne
                //Console.WriteLine("Slot o numerze: {0}, status: {1}",
                //    _slotIndexList.Keys.ElementAt(i), _slotIndexList.Values.ElementAt(i));
            }

            //routingLine = new RoutingLine(listeningPort, sendingPort);
         
        }

        public RoutingLine GetRoutingLine()
        {
            return routingLine;
        }

        public Tuple<string, string> GetConnectedNodes()
        {
            return _nodes;
        }
        /*
         * Zwraca informacje o Linku w formie czytelnej dla  użytkownika
         */
        public string GetLinkToShow()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_nodes.Item1);
            builder.Append("-");
            builder.Append(_nodes.Item2);
            builder.Append(", ");
            builder.Append(_length);
            builder.Append(" - length");
            builder.Append(_bandWidth);
            builder.Append(" - bandWidth");
            builder.Append(_slotNumber);
            builder.Append(" - slotNumber");
            return builder.ToString();
        }
        /*
         * Zwraca informacje o Linku w formie do wysłanie
         */
        public string GetLinkToSend()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(_nodes.Item1);
            builder.Append(",");
            builder.Append(_nodes.Item2);
            builder.Append(",");
            builder.Append(_length);
            builder.Append(",");
            builder.Append(_bandWidth);
            builder.Append(",");
            builder.Append(_slotNumber);
            builder.Append(",");
            return builder.ToString();
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
