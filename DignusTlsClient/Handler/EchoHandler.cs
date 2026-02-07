using Dignus.Sockets.Interfaces;
using System.Text.Json;

namespace DignusTlsClient.Handler
{
    internal class EchoHandler : IProtocolHandler<string>, ISessionComponent
    {
        private ISession _session;
        public double MaxRttMs { get; private set; } = -1;
        public double MinRttMs { get; private set; } = 999999;

        private long _receivedCount;
        private DateTime _lastSendTime;

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
            Monitor.Instance.AddReceivedCount(_receivedCount);
            Monitor.Instance.SetMaxRttMs(MaxRttMs);
            Monitor.Instance.SetMinRttMs(MinRttMs);
            Monitor.Instance.AddClientCount(1);
            _session = null;
        }
    }
}
