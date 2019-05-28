using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    /*
     * CallController po otrzymaniu żądania zestawienia połączenia od hosta:
     * 1. Pyta RC o topologię sieci
     * 2. Znajduje najkrótszą śćieżkę.
     * 3. Wybiera modulację.
     * 4. CC w każdym węźle alokuję zasoby, jak się nie uda to komunikat, że nie da rady zestawić połączenia.
     * 5. Informuję hosta, że połączenie jest gotowe.
     */
    static class CallController
    {
        private static int trailLength; // długość szlaku od nadawcy do odbiorcy
        private static Dictionary<string, int> modulation = new Dictionary<string, int>(); //lista rodzajów modulacji <nazwa, ilosc wartosci>
        private static double bandWidth; // żądana przepustowość
        private static Node node;

        /*
         * Metoda uzupełnia słownik modulacji i zapisuje referencje do węzła
         */
        public static void InitiateCC(Node n)
        {
            node = n;

            modulation.Add("64QAM", 64);
            modulation.Add("32QAM", 32);
            modulation.Add("16QAM", 16);
            modulation.Add("8QAM", 8);
            modulation.Add("QPSK", 4);
            modulation.Add("BPSK", 2);
            modulation.Add("Brak modulacji", 1);
        }
        /*
         * Wybiera modulację, z której będzie korzystać połączenie, zwraca ilość potrzebnych slotów.
         * if (trailLength < 100) modulacja to 64QAM, więc potrzebne pasmo zmniejszy
         */
        public static int SelectModulation()
        {
            for (int i = 1; i < modulation.Count; i++)
                if (trailLength < i * 100) return (int)Math.Ceiling(bandWidth / modulation.ElementAt(i - 1).Value);
            return (int)Math.Ceiling(bandWidth / modulation.ElementAt(modulation.Count - 1).Value);

            //if (trailLength < 100) return (int) Math.Ceiling(trailLength / modulation.ElementAt(0).Value);
            //if (trailLength < 200) return (int)Math.Ceiling(trailLength / modulation.ElementAt(1).Value);
            //if (trailLength < 300) return (int)Math.Ceiling(trailLength / modulation.ElementAt(2).Value);
            //if (trailLength < 400) return (int)Math.Ceiling(trailLength / modulation.ElementAt(3).Value);
            //if (trailLength < 500) return (int)Math.Ceiling(trailLength / modulation.ElementAt(0).Value);
            //if (trailLength < 600) return (int)Math.Ceiling(trailLength / modulation.ElementAt(0).Value);
            //else return (int)Math.Ceiling(trailLength / modulation.ElementAt(0).Value);
        }
    }
}
