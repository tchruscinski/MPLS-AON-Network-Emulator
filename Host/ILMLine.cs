using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    /*
     * Klasa reprezentuje wiersz tablicy ILM
     */
    class ILMLine
    {
        private int _label; //etykieta przychodzaca, 
        private string _sender; //nazwa nadawcy
        public ILMLine(int label, string sender)
        {
            _label = label;
            _sender = sender;
        }

        public int GetLabel() { return _label; }
        public string GetSender() { return _sender; }
    }
}
