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
        * Metoda zwracająca konfigurację z wybranego pliku dla danego routera
        * @routerName - string, nazwa routera, fileName - string, nazwa pliku z konfiguracją
        */
        public string ParseLocalConfig(string fileName, string routerName)
        {
            if (routerName == null || fileName == null)
            {
                return null;
            }

            string returnedString = "";
            LoadFile(fileName);
            XmlNodeList nodesList = config.SelectNodes("/Config/Router");

            foreach (XmlNode node in nodesList)
            {
                if (node["Name"]?.InnerText == routerName)
                {
                    XmlNodeList rowsList = config.SelectNodes("/Config/Router/Row");
                    foreach (XmlNode row in rowsList)
                    {
                        if (row.Attributes["Assigned"]?.InnerText == routerName)
                        {
                            returnedString += row["NHLFE_ID_MPLS"]?.InnerText + ",";
                            returnedString += row["Action"]?.InnerText + ",";
                            returnedString += row["OutLabel"]?.InnerText + ",";
                            returnedString += row["OutPortN"]?.InnerText + ",";
                            returnedString += row["NextID"]?.InnerText + ",";
                            returnedString += row["IncPort"]?.InnerText + ",";
                            returnedString += row["IncLabel"]?.InnerText + ",";
                            returnedString += row["PoppedLabelStack"]?.InnerText + ",";
                            returnedString += row["NHLFE_ID_ILM"]?.InnerText + ",";
                        }
                    }
                }
            }
            returnedString = returnedString.Remove(returnedString.Length - 1);
            return returnedString;
        }
    }
}