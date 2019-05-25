﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Node
{
    /**
    * Klasa tworząca obiekt Parser, który parsuje konfigurację z pliku xml
    * @Parser
    */
    class Parser
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
            XmlNodeList nodesList = config.SelectNodes("/Config/Router");

            foreach (XmlNode node in nodesList)
            {
                XmlNodeList rowsList = config.SelectNodes("/Config/Router/Row");
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
        * Metoda zwracająca konfigurację z wybranego pliku dla danego routera
        * @fileName - string, nazwa pliku z konfiguracją
        */
        public List<string> ParseConfig(string fileName)
        {
            if(fileName == null)
            {
                return null;
            }

            int rowCounter = 0;
            string returnedString = "";
            LoadFile(fileName);
            XmlNodeList nodesList = config.SelectNodes("/Config/Router");

            foreach (XmlNode node in nodesList)
            {
                XmlNodeList rowsList = config.SelectNodes("/Config/Router/Row");
                foreach (XmlNode row in rowsList)
                {                  
                        returnedString += row["Destination"]?.InnerText + ",";
                        returnedString += row["Length"]?.InnerText + ",";
                        returnedString += row["BandWidth"]?.InnerText + ",";
                        returnedString += row["NumberOfRoutingLines"]?.InnerText + ",";
                        
                        if(Int32.Parse(row["NumberOfRoutingLines"]?.InnerText) > 0)
                        {
                            XmlNodeList routingLinesList = config.SelectNodes("/Config/Router/Row/RoutingLine");
                            foreach (XmlNode line in routingLinesList)
                            {
                                returnedString += line["ListeningPort"]?.InnerText + ",";
                                returnedString += line["SendingPort"]?.InnerText + ",";
                            }
                        }
                        rowCounter++;
                }
                Console.WriteLine("row+: " + returnedString);
            }
            if (returnedString.Equals(""))
            {
                return null;
            }
            returnedString = returnedString.Remove(returnedString.Length - 1);
            return new List<string>{returnedString, rowCounter.ToString()};
        }
    }
}
