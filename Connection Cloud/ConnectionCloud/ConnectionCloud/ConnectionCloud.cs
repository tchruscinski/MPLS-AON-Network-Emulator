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
        private static CableEmulatorTableParser cetParser = new CableEmulatorTableParser();
        private String[] _processedText;
        Time time = new Time();

        //TODO - wypelnianie tabeli routingu danymi
        public String[] ReadPacket(string packet)
        {
            _packet = packet;
            _processedText = _textProcessor.splitText(_packet);
            if (_processedText[0] == "TAB" && (_processedText.Length == 4))
            {
                Console.WriteLine(time.GetTimestamp(DateTime.Now) + "ConnectionCloud: Received new packet: ");
                Console.WriteLine("Hostname: {0}, Port: {1}, Label: {2}", _processedText[1], _processedText[2], _processedText[3]);
                return _processedText;

                //[2019-04-10 22:21:03] Created UDPServer at: 127.0.0.1:21370
                //[2019-04-10 22:21:20] RECV: 127.0.0.1:62523: 24
                //20,17,;wiadomosc testowa

            }
            else 
            {
                return _processedText;
            }
        }

        public void FillRoutingTable(String[] data)
        {

        }

        private static string ReadCloudConfig(string routerName)
        {
            string routerConfig = cetParser.ParseCableCloudEmulatorTable("cloud_config.xml", routerName);

            Console.WriteLine("sparsowany xml: " + routerConfig);
            Console.ReadKey();

            return routerConfig;
        }

    }
}
