using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    /*
     * Klasa reprezentuje wiersz tablicy routingowej routera
     * Zawiera nr portu, nazwe hosta i etykiete mpls(?)
     * Tez moze byc sytuacja, ze w danej chwili danym portem nie bedziemy kierowac zadnych pakietow,
     * wiec host bedzie nullem(?)
     * 
     * Jesli np. tablica wyglada 10 | A, to znaczy, ze jesli chcemy wyslac pakiet do hosta A, 
     * musimy kierowac go do portu nr 10
     */
    class RoutingLine
    {
        private int _port; //nr portu
        private string _hostName; //nazwa hosta docelowego

        public RoutingLine(int port, string hostName="null")
        {
            _port = port;
            _hostName = hostName;
        }
    }
}