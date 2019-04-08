using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionCloud
{
    public class ConnectionCloud
    {
        private List<RoutingTableLine> routingTable = new List<RoutingTableLine>(); //FIB(?)
        //sockety, ktorymi pakiety sa przesylane dalej
        private List<UDPSocket> sendingSockets = new List<UDPSocket>();
        //sockety, ktore odbieraja pakiety
        private List<UDPSocket> receivingSockets = new List<UDPSocket>();
        private String _packet = " "; //tresc pakietu obslugiwanego w danym momencie przez chmurę,
        private String destinationHost = " "; //docelowy host pakietu obslugiwanego
        private TextProcessor _textProcessor = new TextProcessor();
        private String[] _processedText;
        Time time = new Time();

        //TODO - wypelnianie tabeli routingu danymi
        public String[] ReadPacket(string packet)
        {
            _packet = packet;
            _processedText = _textProcessor.splitText(_packet);
            if (_processedText[0] == "TAB" && (_processedText.Length == 4))
            {
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + "ConnectionCloud: Received new routing table data: ");
                Console.WriteLine("Hostname: {0}, Port: {1}, Label: {2}", _processedText[1], _processedText[2], _processedText[3]);
                return _processedText;
            }
            else
            {
                return _processedText;
            }
        }

        public void FillRoutingTable(String[] data)
        {

        }

    }
}
