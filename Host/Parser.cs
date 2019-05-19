using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Host
{
    /**
    * Klasa tworząca obiekt Parser, który parsuje konfigurację z pliku xml
    * @Parser
    */
    public class Parser
    {
        private static XmlDocument config = new XmlDocument();
        private static XmlNode root = config.FirstChild;

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
        * Metoda zwracająca konfigurację z wybranego pliku dla danego hosta
        * @fileName - string, nazwa pliku z konfiguracją
        */
        public string ParseLocalConfig(string fileName)
        {
            if (fileName == null)
            {
                return null;
            }

            string returnedString = "";
            LoadFile(fileName);
            XmlNodeList nodesList = config.SelectNodes("/Config/Host");

            foreach (XmlNode node in nodesList)
            {
                XmlNodeList rowsList = config.SelectNodes("/Config/Host/Row");
                foreach (XmlNode row in rowsList)
                {     
                    returnedString += row["Type"]?.InnerText + ",";
                    returnedString += row["Port"]?.InnerText + ",";                    
                }
          
            }

            if (returnedString.Equals(""))
            {
                return null;
            }
            returnedString = returnedString.Remove(returnedString.Length - 1);
            return returnedString;
        }

        /**
        * Metoda zwracająca konfigurację z wybranego pliku dla danego hosta
        * @hostName - string, nazwa hosta, fileName - string, nazwa pliku z konfiguracją
        */
        public string ParseHostTable(string fileName, string hostName)
        {
            if (hostName == null || fileName == null)
            {
                return null;
            }

            string returnedString = "";
            LoadFile(fileName);
            XmlNodeList nodesList = config.SelectNodes("/Config/Host");

            foreach (XmlNode node in nodesList)
            {
                if (node["Name"]?.InnerText == hostName)
                {
                    XmlNodeList rowsList = config.SelectNodes("/Config/Host/Row");
                    foreach (XmlNode row in rowsList)
                    {
                        if (row.Attributes["Assigned"]?.InnerText == hostName)
                        {
                            returnedString += row["DestinationHost"]?.InnerText + ",";
                            returnedString += row["NHLFE_ID"]?.InnerText + ",";
                            returnedString += row["Label"]?.InnerText + ",";
                            returnedString += row["Sender"]?.InnerText + ",";
                            returnedString += row["ID"]?.InnerText + ",";
                            returnedString += row["NLabel"]?.InnerText + ",";
                            returnedString += row["NextId"]?.InnerText + ",";
                        }
                    }
                }
            }
            if (returnedString.Equals(""))
            {
                return null;
            }
            returnedString = returnedString.Remove(returnedString.Length - 1);
            return returnedString;
        }
    }
}