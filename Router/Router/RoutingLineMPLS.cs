using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    /*
     * Klasa reprezentuje wiersz tablicy FIB-MPLS routera
     */
    class RoutingLineMPLS
    {
        string _destinationHost; //host docelowy
        int _valueFEC; //wartosc FEC

        public RoutingLineMPLS(string destinationHost, int valueFEC)
        {
            _destinationHost = destinationHost;
            _valueFEC = valueFEC;
        }
        public String GetHostName() { return _destinationHost; }
        public int GetFEC() { return _valueFEC; }
    }

}
