using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    /*
     * Klasa reprezentuje wiersz tablicy FTN
     * zawiera wskaznik do wpisow NHLFE, dla danego numeru FEC
     */
    class FTNLine
    {
        private int _valueFEC;
        private int _idNHLFE;
        public FTNLine(int valueFEC, int idNHLFE)
        {
            _valueFEC = valueFEC;
            _idNHLFE = idNHLFE;
        }
        public int GetId() { return _idNHLFE; }
        public int GetFEC() { return _valueFEC; }
    }
}
