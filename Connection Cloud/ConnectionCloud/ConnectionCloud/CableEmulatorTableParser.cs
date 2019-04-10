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
        public string ParseCableCloudEmulatorTable(string fileName, string rowId)
        {
            if (rowId == null || fileName == null)
            {
                return null;
            }

            string returnedString = "";
            LoadFile(fileName);
            XmlNodeList nodesList = config.SelectNodes("/Config/Cloud");

            foreach (XmlNode node in nodesList)
            {
                if (node["rowId"]?.InnerText == rowId)
                {
                    XmlNodeList rowsList = config.SelectNodes("/Config/Router/Row");
                    foreach (XmlNode row in rowsList)
                    {
                        returnedString += row["incomingPort"]?.InnerText + ",";
                        returnedString += row["incomingLabel"]?.InnerText + ",";
                        returnedString += row["outgoingPort"]?.InnerText + ",";
                        returnedString += row["outgoingLabe"]?.InnerText + ",";
                        
                    }
                }
            }
            return returnedString;
        }

        /**
        * Metoda zwracająca konfigurację z wybranego pliku dla danego hosta
        * @hostName - string, nazwa hosta, fileName - string, nazwa pliku z konfiguracją
        */
        //public string ParseHostTable(string fileName, string hostName)
        //{
        //    if (hostName == null || fileName == null)
        //    {
        //        return null;
        //    }

        //    string returnedString = "";
        //    LoadFile(fileName);
        //    XmlNodeList nodesList = config.SelectNodes("/Config/Host");

        //    foreach (XmlNode node in nodesList)
        //    {
        //        if (node["Name"]?.InnerText == hostName)
        //        {
        //            XmlNodeList rowsList = config.SelectNodes("/Config/Host/Row");
        //            foreach (XmlNode row in rowsList)
        //            {
        //                returnedString += row["DestinationHost"]?.InnerText + ",";
        //                returnedString += row["NHLFE_ID"]?.InnerText + ",";
        //            }
        //        }
        //    }
        //    return returnedString;
        //}
    }
}