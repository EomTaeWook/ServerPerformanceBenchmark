using Dignus.Sockets.Attributes;
using Dignus.Sockets.Interfaces;
using DignusEchoServer.Packets;
using DignusEchoServer.Protocol;
using System.Text.Json;

namespace DignusEchoServer.Handler
{
    internal class EchoHandler : IProtocolHandler<string>, ISessionComponent
    {
        private ISession _session;
        public T DeserializeBody<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body);
        }

        [ProtocolName("EchoMessage")]
        public void Process(EchoMessage echo)
        {
            var body = JsonSerializer.Serialize(echo);
            _session.TrySend(new Packet((int)SCProtocol.EchoMessageResponse, body));
        }

        public void OtherMessage(OtherMessage otherMessage)
        {
            var body = JsonSerializer.Serialize(otherMessage);
            _session.TrySend(new Packet((int)SCProtocol.OtherMessageResponse, body));
        }

        public void SetSession(ISession session)
        {
            _session = session;
        }

        public void Dispose()
        {
            _session = null;
        }

    }
}
