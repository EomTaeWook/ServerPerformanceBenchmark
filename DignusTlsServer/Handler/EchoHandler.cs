using Dignus.Sockets.Interfaces;
using System.Text.Json;

namespace DignusTlsServer.Handler
{
    internal class EchoHandler : IProtocolHandler<string>, ISessionComponent
    {
        private ISession _session;
        public T DeserializeBody<T>(string body)
        {
            return JsonSerializer.Deserialize<T>(body);
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
