using Dignus.Sockets.Interfaces;
using Dignus.Sockets.Udp;

namespace UdpEchoClient
{
    internal class ClientModule : UdpClientBase
    {
        private bool _isConnect = false;
        private ISession _session;
        public ClientModule(UdpSessionConfiguration udpSessionConfiguration) : base(udpSessionConfiguration)
        {
        }

        protected override void OnConnected(ISession session)
        {
            _session = session;
            _isConnect = true;
        }

        protected override void OnDisconnected(ISession session)
        {
            _isConnect = false;
        }

        public void SendMessage(byte[] message, int count)
        {
            for (int i = 0; i < count; i++)
            {
                _session.SendAsync(message);
            }
        }
    }
}
