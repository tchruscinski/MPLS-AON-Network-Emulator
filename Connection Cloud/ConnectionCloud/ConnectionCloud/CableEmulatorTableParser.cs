using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace ConnectionCloud
{
    /**
    * Klasa tworząca obiekt Parser, który parsuje konfigurację z pliku xml
    * @Parser
    */
    public class CableEmulatorTableParser
    {
        private static XmlDocument config = new XmlDocument();
        //private static XmlNode root = config.FirstChild;

        /**
        * Metoda ładująca plik (inicjująca zmienną config), który następnie będzie parsowany
        * @fileName - string, nazwa pliku do załadowania
        */
        public void LoadFile(string fileName)
        {
            try
            {
                config.Load(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        

        /**
        * Metoda sciagajaca chmure XD
        * Troche jeszcze do dokoncznia ale wale w kime elo
        */
        public string ParseCableCloudEmulatorTable(/*string fileName, string rowId*/)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(@"C:/Users/Mikolaj/Desktop/TSST_projekt/tsst-network-emulator/Connection Cloud/ConnectionCloud/ConnectionCloud/cloud_config.xml");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Cloud/Row");

            List<RoutingTableLine> rtl = new List<RoutingTableLine>();

            foreach (XmlNode node in nodes)
            {
                RoutingTableLine routingTableLine = new RoutingTableLine(); 

                routingTableLine._incomingPort = node.SelectSingleNode("incomingPort").InnerText;
                routingTableLine._incomingLabel = node.SelectSingleNode("incomingLabel").InnerText;
                routingTableLine._outgoingPort = node.SelectSingleNode("outgoingPort").InnerText;
                routingTableLine._outgoingLabel = node.SelectSingleNode("outgoingLabel").InnerText;
                

                rtl.Add(routingTableLine);
            }

            System.Console.WriteLine("Total RTLs: " + rtl.Count);
            return rtl[1]._incomingPort;
        }

    }
    class Book
    {
        public string id;
        public string title;
        public string author;
    }
}