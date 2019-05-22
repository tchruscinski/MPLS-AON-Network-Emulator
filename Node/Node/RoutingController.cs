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

        /*
         * Po utworzeniu instancji węzła lista łączy jest uzupełniana danymi od LRM
         */ 
        public static void SetInitialLinks(List<Link> links)
        {
            _links = links;
        }
        
    }
}
