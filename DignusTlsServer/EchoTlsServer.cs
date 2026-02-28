using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using Dignus.Sockets.Tls;

namespace DignusTlsServer
{
    internal class EchoTlsServer : TlsServerBase
    {
        public EchoTlsServer(SessionConfiguration sessionConfiguration, TlsServerOptions tlsServerOptions) : base(sessionConfiguration, tlsServerOptions)
        {
        }
        protected override void OnAccepted(ISession session)
        {
            //LogHelper.Info($"[server] session accepted - {session.Id}");
        }

        protected override void OnDisconnected(ISession session)
        {
            //LogHelper.Info($"[server] session disconnected - {session.Id}");
        }

        protected override void OnHandshaking(ISession session)
        {
        }
        protected override void OnHandshakeFailed(ISession session, Exception ex)
        {
            base.OnHandshakeFailed(session, ex);
            LogHelper.Error($"[server] session disconnected - {session.Id}");
        }
    }
}
