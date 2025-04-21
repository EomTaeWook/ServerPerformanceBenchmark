using Dignus.Sockets;
using Dignus.Sockets.Interfaces;

namespace IndividualServer
{
    internal class Server : ServerBase
    {
        public Server(SessionConfiguration sessionConfiguration) : base(sessionConfiguration)
        {
        }

        protected override void OnAccepted(ISession session)
        {

        }

        protected override void OnDisconnected(ISession session)
        {

        }
    }
}
