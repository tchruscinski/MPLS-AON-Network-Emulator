using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    /*
     * Klasa reprezentuje wiersz tablicy FIB-MPLS routera
     */
    class RoutingLineMPLS
    {
        int _label; //etykieta, ktora zostanie nalozona na pakiet przy wysylaniu
        int _port; //port, ktorym pakiet zostanie wyslany
    }
}
