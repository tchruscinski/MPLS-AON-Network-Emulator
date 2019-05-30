using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    /*
     * Routing controller, wymienia się z innymi węzłami informacjami o topologii sieci.
     * Po otrzymaniu nowych informacji, od LRM lub RC innego węzła przesyła informacje do innych węzłów.
     */
    static class RoutingController
    {
        private static List<Link> _links = new List<Link>(); //lista wszystkich łączy o jakich wie RC
        private static bool _wasChange = false; //czy RC dostał jakieś nowe informacje
        private static Node node;

        public static void SetNode(Node n) { node = n; }
        public static void AddLink(Link link) { _links.Add(link); }
        public static List<Link> GetLinks() { return _links; }

        /*
         * Po utworzeniu instancji węzła lista łączy jest uzupełniana danymi od LRM i przesyła sąsiadom
         */
        public static void SetInitialLinks(List<Link> links)
        {
            _links = links;
            StringBuilder linksBuilder = new StringBuilder();
            foreach (Link i in _links)
                linksBuilder.Append(i.GetLinkToSend());
            foreach (UDPSocket i in node.GetSendingSockets())
            {
                i.Send(linksBuilder.ToString());
            }


        }
        /*
         * Porównuje dwa łącza, jeżeli mają identyczne parametry zwraca true
        */
        public static bool CompareLinks(Link link1, Link link2)
        {
            if (link1.GetNode1().Equals(link2.GetNode1()) && link1.GetNode2().Equals(link2.GetNode2())
                && link1.GetLength() == link2.GetLength() && link1.GetCapacity() == link2.GetCapacity())
                return true;
            else
                return false;
        }
        public static void SetWasChange(bool wasChange) { _wasChange = wasChange; }
        /*
         * Sprawdza informacje o łączach otrzymane od innych węzłów, jeśli dowiedział się czegoś nowego, zwraca true
         * oraz zapamiętuje te informacje
         */
        public static bool CheckNewLinks(List<Link> newLinks)
        {
            /*
             * _links - łącza, o których wie
             * newLinks - łącza, które otrzymał
             * każde nowe łącze jest porównywane ze wszystkimi starymi
             */

            bool wasChange = false;
            for (int i = 0; i < newLinks.Count; i++)
                for (int j = 0; j < _links.Count; j++) {
                    if (RoutingController.CompareLinks(newLinks[i], _links[j]))
                    {
                        //jeżeli jest już takie same łącze, to nie ma sensu dalej porównywać
                        break;
                    }
                    if (j == _links.Count - 1)
                    {
                        //jak doszliśmy do końca i nie było identycznego, to znaczy, że należy go dodać
                        _links.Add(newLinks[i]);
                        wasChange = true;
                    }
                }

            return wasChange;

        }


    }
}
