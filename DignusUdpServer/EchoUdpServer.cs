using Dignus.Sockets.Interfaces;
using Dignus.Sockets.Udp;
using System;
using System.Collections.Generic;
using System.Text;

namespace DignusUdpServer
{
    internal class EchoUdpServer : UdpServerBase
    {
        public EchoUdpServer(UdpSessionConfiguration sessionConfiguration, UdpServerOption udpServerOption) : base(sessionConfiguration, udpServerOption)
        {
        }

        protected override void OnConnected(ISession session)
        {
            
        }

        protected override void OnDisconnected(ISession session)
        {
            
        }
    }
}
