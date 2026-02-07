using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using Dignus.Sockets.Tls;
using DignusTlsClient.Handler;
using System;

namespace DignusTlsClient
{
    internal class ClientModule : TlsClientBase
    {
        private bool _isConnect = false;
        private ISession _session;
        private EchoHandler _echoHandler;

        public ClientModule(SessionConfiguration sessionConfiguration, TlsClientOptions tlsClientOptions) : base(sessionConfiguration, tlsClientOptions)
        {
        }
        public void SendMessage(byte[] message, int count)
        {
            for (int i = 0; i < count; i++)
            {
                //Send(message);
                _session.SendAsync(message);
            }
        }

        protected override void OnConnected(ISession session)
        {
            _session = session;
            foreach (var component in session.GetSessionComponents())
            {
                if (component is EchoHandler echoHandler)
                {
                    _echoHandler = echoHandler;
                }
            }
            _isConnect = true;
            SendMessage(Consts.Message, 1000);
        }
        protected override void OnHandshaking(ISession session)
        {
            base.OnHandshaking(session);
        }
        protected override void OnDisconnected(ISession session)
        {
            _isConnect = false;
        }
        public bool IsConnect => _isConnect;
    }
}
