using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using EchoClient.Handler;
using EchoClient.Serializer;

namespace EchoClient
{
    internal class ClientModule : ClientBase
    {
        private bool _isConnect = false;
        private ISession _session;
        private EchoHandler _echoHandler;
        private EchoSerializer _echoSerializer;
        public ClientModule(SessionConfiguration sessionConfiguration) : base(sessionConfiguration)
        {
        }
        public void SendEcho(string message)
        {
            _echoHandler.SendEcho(message);
        }
        public void SendMessage(byte[] message, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    _echoSerializer.SendMessage(message);
                });
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
                if (component is EchoSerializer echoSerializer)
                {
                    _echoSerializer = echoSerializer;
                }
            }
            _isConnect = true;
        }

        protected override void OnDisconnected(ISession session)
        {
            _isConnect = false;
        }
        public bool IsConnect => _isConnect;
    }
}
