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
    class MPLSLine
    {
        string _destinationHost; //host docelowy
        int _valueNHLFE; //wartosc NHLFE ID

        public MPLSLine(string destinationHost, int valueNHLFE)
        {
            _destinationHost = destinationHost;
            _valueNHLFE = valueNHLFE;
        }
        public String GetHostName() { return _destinationHost; }
        public int GetNHLFE() { return _valueNHLFE; }
    }

}
