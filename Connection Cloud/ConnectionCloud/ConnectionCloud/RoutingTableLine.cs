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
        private int _incomingPort; //nr portu nadawcy
        private string _incomingHostName; //nazwa hosta nadawcy
        private string _incomingLabel; //etykieta pakietu od nadawcy 

        private int _outgoingPort; //nr portu odbiorcy
        private string _outgoingHostName; //nazwa hosta odbiorcy
        private string _outgoingLabel; //etykieta pakietu od odbiorcy 

        public RoutingTableLine(int incomingPort, string incomingHostName, string incomingLabel, int outgoingPort, string outgoingHostName, string outgoingLabel)
        {
            _incomingPort = incomingPort;
            _incomingHostName = incomingHostName;
            _incomingLabel = incomingLabel;

            _outgoingHostName = outgoingHostName;
            _outgoingLabel = outgoingLabel;
            _outgoingPort = outgoingPort;

        }

        public String GetIncomingHostName() { return _incomingHostName; }
        public int GetIncomingPort() { return _incomingPort; }
        public String GetIncomingLabel() { return _incomingLabel; }

        public String GetOutgoingHostName() { return _outgoingHostName; }
        public int GetOutgoingPort() { return _outgoingPort; }
        public String GetOutgoingLabel() { return _outgoingLabel; }
    }
}
