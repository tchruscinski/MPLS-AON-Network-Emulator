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
        public List<RoutingTableLine> ParseCableCloudEmulatorTable(/*string fileName, string rowId*/)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(@"../../cloud_config.xml");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Cloud/Row");

            List<RoutingTableLine> rtl = new List<RoutingTableLine>();

            int i = 0;
            foreach (XmlNode node in nodes)
            {
                RoutingTableLine rtl_obj = new RoutingTableLine();

                string inP = " ";
                string outP = " ";

                inP = node.SelectSingleNode("incomingPort").InnerText;
                outP = node.SelectSingleNode("outgoingPort").InnerText;

                rtl_obj._incomingPort = Int32.Parse(inP);
                rtl_obj._outgoingPort = Int32.Parse(outP);
                rtl.Add(rtl_obj);
                
            }
            Console.WriteLine("{0}", rtl[2]._outgoingPort);
            Console.WriteLine("{0}", rtl[3]._outgoingPort);

            System.Console.WriteLine("Total RTLs: " + rtl.Count);
            return rtl;
        }

    }
}