using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Management_System
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
        * Metoda parsująca wybrany węzeł i atrybut z pliku konfiguracyjnego xml
        * @nodeName - string, nazwa szukanego węzłą, attributeName - string, nazwa szukanego atrybutu
        */
        /*public List<string> ParseConfig(string fileName, string nodeName, string attributeName)
        {
            LoadFile(fileName);
            XmlNode root = config.DocumentElement;
            XmlNodeList pickedNodes = root.SelectNodes(nodeName);
            List<string> returnedValues = new List<string>();

            foreach (XmlNode node in pickedNodes)
            {
                if (node.Attributes[attributeName]?.InnerText != null)
                {
                    returnedValues.Add(node.Attributes[attributeName].InnerText);
                }
            }

            if (returnedValues.Count == 0)
            {
                return null;
            }
            return returnedValues;
        }*/

        /**
        * Metoda zwracająca konfigurację z wybranego pliku dla danego routera
        * @routerName - string, nazwa routera, fileName - string, nazwa pliku z konfiguracją
        */
        public string ParseRouterTable(string fileName, string routerName)
        {
            if(routerName == null || fileName == null)
            {
                return null;
            }

            string returnedString = "";
            LoadFile(fileName);
            XmlNodeList nodesList = config.SelectNodes("/Config/Router");

            foreach (XmlNode node in nodesList)
            {
                if(node["Name"]?.InnerText == routerName)
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
                            returnedString += row["NLabel"]?.InnerText + ",";
                            returnedString += row["NextId"]?.InnerText + ",";
                        }
                    }
                }
            }
            return returnedString;
        }
    }
}