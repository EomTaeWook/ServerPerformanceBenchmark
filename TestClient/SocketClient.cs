using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using System.Text;

namespace TestClient
{
    internal class SocketClient : ClientBase
    {
        public SocketClient(SessionConfiguration sessionConfiguration) : base(sessionConfiguration)
        {
        }

        protected override void OnConnected(ISession session)
        {
            //var bytes = Encoding.UTF8.GetBytes("ping");
            //session.Send(bytes);
        }

        protected override void OnDisconnected(ISession session)
        {

        }
    }
}
