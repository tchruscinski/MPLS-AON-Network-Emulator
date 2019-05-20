using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouterV1
{
    /*
     * Link resource manager, posiada informacje o łączach połączonych z danym węzłem i ich stanie
     * przekazuje te wiedze do RC
     */
    static class LinkResourceManager
    {
        private static List<Link> links = new List<Link>();

        public static List<Link> GetLinks() { return links; }

    }
}
