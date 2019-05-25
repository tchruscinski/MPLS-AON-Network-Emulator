using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    /*
     * Klasa reprezentuje sk³¹dow¹ ³¹cza miêdzy wêz³ami, pojedyñcz¹ linie po³¹czeñ
     */
    public class RoutingLine
    {
        private int listeningPort;
        private int sendingPort;
        private UDPSocket listeningSocket = new UDPSocket();
        private UDPSocket sendingSocket = new UDPSocket();

        public RoutingLine(int lP, int sP)
        {
            listeningPort = lP;
            sendingPort = sP;
            listeningSocket.SetPort(listeningPort);
            sendingSocket.SetPort(sendingPort);
        }

    }
}