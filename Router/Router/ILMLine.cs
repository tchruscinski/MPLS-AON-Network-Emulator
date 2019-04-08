using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    /*
     * Klasa reprezentuje wiersz tablicy ILM
     */
    class ILMLine
    {
        private int _port; //nr portu, z ktorego przyszedl pakiet
        private int _label; //etykieta przychodzaca, etykieta na szczycie stosu etykiet pakietu
        private List<int> _poppedLabels = new List<int>(); //zdjete etykiety
        private int _valueNHLFE; //id wiersza nhlfe
        public ILMLine(int port, int label, List<int> poppedLabels, int valueNHLFE)
        {
            _port = port;
            _label = label;
            _poppedLabels = poppedLabels;
            _valueNHLFE = valueNHLFE;
        }
    }
}
