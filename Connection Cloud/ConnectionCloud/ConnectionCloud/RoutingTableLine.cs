using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionCloud

{
    /*
     * Klasa reprezentuje wiersz tablicy routingowej chmury polaczen
     * Zawiera nr portu, nazwe hosta i etykiete mpls(?) nadawcy i odbiorcy
     * Tez moze byc sytuacja, ze danym portem nie bedziemy kierowac zadnych pakietow,
     * wiec host bedzie nullem(?)
     * 
     * Jesli np. tablica wyglada 10 | A, to znaczy, ze jesli chcemy wyslac pakiet do hosta A, 
     * musimy kierowac go do portu nr 10
     */
    public class RoutingTableLine
    {
        public int _incomingPort; //nr portu nadawcy 
        public int _outgoingPort; //nr portu odbiorcy 
    }
}
