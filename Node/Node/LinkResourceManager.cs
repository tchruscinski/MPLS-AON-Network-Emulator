using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    /*
     * Link resource manager, posiada informacje o łączach połączonych z danym węzłem i ich stanie
     * przekazuje te wiedze do RC
     */
    public static class LinkResourceManager
    {
        private static List<Link> links = new List<Link>();

        public static List<Link> GetLinks() { return links; }

        /*
         * Metoda dodaje nowe łącze pomiędzy zdefiniowanymi węzłami o określonych parametrach do Link Resource Managera
         *
         * @node1 - nazwa jednego konćowego węzła
         * @node2 - nazwa drugiego konćowego węzła
         * @length - długość łącza
         * @bandWidth - przepustowość łącza
         */
        public static void AddLink(string node1, string node2, int length, double bandWidth)
        {
            links.Add(new Link(node1, node2, length, bandWidth));
        }

        /*
         * Metoda dodaje nowe łącze do Link Resource Managera
         *
         */
        public static void AddLink(Link link)
        {
            links.Add(link);
        }
    }
}
