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

        /**
        * Konstruktor inicjucjący atrybut config obiektu, czyli dokument xml oraz listę węzłów w dokumencie
        * @Parser
        */
        public Parser()
        {
            try
            {
                config.Load("example_config.xml");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /**
        * Metoda parsująca wybrany węzeł i atrybut z pliku konfiguracyjnego xml
        * @nodeName - string, nazwa szukanego węzłą, attributeName - string, nazwa szukanego atrybutu
        */
        public List<string> ParseConfig(string nodeName, string attributeName)
        {
            XmlNode root = config.DocumentElement;
            XmlNodeList pickedNodes = root.SelectNodes(nodeName);
            List<string> returnedValues = new List<string>();

            foreach(XmlNode node in pickedNodes)
            {
                if (node.Attributes[attributeName]?.InnerText != null)
                {
                    returnedValues.Add(node.Attributes[attributeName].InnerText);
                }     
            }

            if(returnedValues.Count == 0)
            {
                return null;
            }
            return returnedValues;
        }
    }
}