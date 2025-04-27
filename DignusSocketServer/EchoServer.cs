using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using DignusEchoServer.Handler;
using DignusEchoServer.Protocol;

namespace DignusEchoServer
{
    internal class EchoServer : ServerBase
    {
        public EchoServer(SessionConfiguration sessionConfiguration) : base(sessionConfiguration)
        {
            ProtocolHandlerMapper<EchoHandler, string>.BindProtocol<CSProtocol>();
        }
        protected override void OnAccepted(ISession session)
        {
            //LogHelper.Info($"[server] session accepted - {session.Id}");
        }

        protected override void OnDisconnected(ISession session)
        {
            //LogHelper.Info($"[server] session disconnected - {session.Id}");
        }
    }
}
