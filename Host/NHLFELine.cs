using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    /*
     * Klasa reprezentuje wiersz tabeli Next Hop Label Forwarding Entry
     * zawiera operacje, ktore musimy wykonac dla danego ID
     */

    class NHLFELine
    {
        private int _ID; //ID wiersza
        private int _label; //nr etykiety, 0 = brak etykiety
        private int _nextID; //ID nastepnego wiersza, 0 = nie ma nastepnego wiersza

        public NHLFELine(int ID, int label, int nextID)
        {
            _ID = ID;
            _label = label;
            _nextID = nextID;
        }
        public int getID() { return _ID; }
        public int getLabel() { return _label; }
        public int getNextID() { return _nextID; }




    }
}
