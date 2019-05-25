using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    /*
     * Klasa reprezentuje sk��dow� ��cza mi�dzy w�z�ami, pojedy�cz� linie po��cze�
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