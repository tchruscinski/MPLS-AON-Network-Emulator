using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionCloud
{
    class ConnectionCloud
    {
        private List<RoutingTableLine> routingTable = new List<RoutingTableLine>(); //FIB(?)
        //sockety, ktorymi pakiety sa przesylane dalej
        private List<UDPSocket> sendingSockets = new List<UDPSocket>();
        //sockety, ktore odbieraja pakiety
        private List<UDPSocket> receivingSockets = new List<UDPSocket>();
        private String _packet = " "; //tresc pakietu obslugiwanego w danym momencie przez chmurę,
        private String destinationHost = " "; //docelowy host pakietu obslugiwanego
        

        //TODO - wypelnianie tabeli routingu danymi 

    }
}
