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
    class RoutingTableLine
    {
        public string _incomingPort; //nr portu nadawcy
        public string _incomingLabel; //etykieta pakietu od nadawcy 

        public string _outgoingPort; //nr portu odbiorcy
        public string _outgoingLabel; //etykieta pakietu od odbiorcy 

        //public RoutingTableLine(int incomingPort, string incomingLabel, int outgoingPort, string outgoingLabel)
        //{
        //    _incomingPort = incomingPort;
        //    _incomingLabel = incomingLabel;

        //    _outgoingLabel = outgoingLabel;
        //    _outgoingPort = outgoingPort;

        //}



        //public int GetIncomingPort() { return _incomingPort; }
        //public String GetIncomingLabel() { return _incomingLabel; }

        //public int GetOutgoingPort() { return _outgoingPort; }
        //public String GetOutgoingLabel() { return _outgoingLabel; }
    }
}
