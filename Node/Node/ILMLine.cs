using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    /*
     * Klasa reprezentuje wiersz tablicy ILM
     */
    class ILMLine
    {
        private int _port; //nr portu, z ktorego przyszedl pakiet
        private int _label; //etykieta przychodzaca, etykieta na szczycie stosu etykiet pakietu
        private string _poppedLabels; //zdjete etykiety
        private int _valueNHLFE; //id wiersza nhlfe
        public ILMLine(int port, int label, string poppedLabels, int valueNHLFE)
        {
            _port = port;
            _label = label;
            _poppedLabels = poppedLabels;
            _valueNHLFE = valueNHLFE;
        }
        public int GetPort() { return _port; }
        public int GetLabel() { return _label; }
        public string GetPoppedLabels() { return _poppedLabels; }
        public int GetNHLFE() { return _valueNHLFE; }
    }
}
