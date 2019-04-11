using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace RouterV1
{
    /**
    * Klasa tworząca obiekt Parser, który parsuje konfigurację z pliku xml
    * @Parser
    */
    class Parser
    {
        private static XmlDocument config = new XmlDocument();
        private static XmlNode root = config.FirstChild;
        private Router _router;
        public Parser(Router router)
        {
            _router = router;
        }
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
        public void ParseSockets(string fileName)
        {
            if (fileName == null) return;
            LoadFile(fileName);
            XmlNodeList nodesList = config.SelectNodes("/Config/Router1");
            foreach (XmlNode node in nodesList)
            {
                XmlNodeList rowsList = config.SelectNodes("/Config/Router1/Row");
                foreach (XmlNode row in nodesList)
                {
                  //TODO

                }

            }

        }
    }
}
