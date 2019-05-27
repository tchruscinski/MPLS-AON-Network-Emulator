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
        private static bool wasChange = false; //czy RC dostał jakieś nowe informacje
        private static Node node;

        public static void SetNode(Node n) { node = n; }

        /*
         * Po utworzeniu instancji węzła lista łączy jest uzupełniana danymi od LRM i przesyła sąsiadom
         */ 
        public static void SetInitialLinks(List<Link> links)
        {
            _links = links;
            StringBuilder linksBuilder = new StringBuilder();
            foreach (Link i in _links)
                linksBuilder.Append(i.GetLinkToSend());
            foreach(UDPSocket i in node.GetSendingSockets())
            {
                i.Send(linksBuilder.ToString());
            }
                

        }
        public static void SetWasChange(bool wc) { wasChange = wc; }

        
    }
}
