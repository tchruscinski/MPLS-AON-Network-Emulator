using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    /*
     * Klasa reprezentuje wiersz tabeli Next Hop Label Forwarding Entry
     * zawiera operacje, ktore musimy wykonac dla danego ID
     */
    enum Action { PUSH, POP, SWAP }; //rodzaj operacji, push -> dodaj, pop -> usun, swap -> zamien

    class NHLFELine
    {
        private int _ID; //ID wiersza
        Action _action;
        private int _label; //nr etykiety, 0 = brak etykiety
        private int _port; //nr portu wyjsciowego, 0 = brak portu
        private int _nextID; //ID nastepnego wiersza, 0 = nie ma nastepnego wiersza

        public NHLFELine(int ID, Action action , int label, int port, int nextID)
        {
            _ID = ID;
            _action = action;
            _label = label;
            _port = port;
            _nextID = nextID;
        }
        public int getID() { return _ID; }
        public Action getAction() { return _action; }
        public int getLabel() { return _label; }
        public int getPort() { return _port; }
        public int getNextID() { return _nextID; }




    }
}
