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
        * Metoda zwracająca konfigurację z wybranego pliku dla danego routera
        * @routerName - string, nazwa routera, fileName - string, nazwa pliku z konfiguracją
        */
        public string ParseCableCloudEmulatorTable(string fileName, string rowId)
        {
            if (rowId == null || fileName == null)
            {
                return null;
            }

            string returnedString = "";
            LoadFile(fileName);
            XmlNodeList nodesList = config.SelectNodes("/Config/Cloud");

            foreach (XmlNode node in config.DocumentElement)
            {
                Console.WriteLine("dupa+{0}", node["Row"]?.InnerText);
                string rId = node.Attributes[0].InnerText;
                if (rId == rowId)
                {
                    XmlNodeList rowsList = config.SelectNodes("/Config/Cloud/Row");
                    foreach (XmlNode row in rowsList)
                    {
                        returnedString += row["incomingPort"]?.InnerText + ",";
                        returnedString += row["incomingLabel"]?.InnerText + ",";
                        returnedString += row["outgoingPort"]?.InnerText + ",";
                        returnedString += row["outgoingLabel"]?.InnerText + ",";
                        
                    }
                }
                else
                {
                    Console.WriteLine("CHUJNIA COS EJJJJ");
                }
            }
            return returnedString;
        }

    }
}