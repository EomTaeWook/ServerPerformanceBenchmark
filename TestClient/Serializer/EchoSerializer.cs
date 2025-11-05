using Dignus.Collections;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using EchoClient.Extensions;
using EchoClient.Packets;

namespace EchoClient.Serializer
{
    internal class EchoSerializer() : IPacketHandler, IPacketSerializer, ISessionComponent
    {
        private long _totalBytes = 0;
        private readonly double _maxRttMs = -1;
        private readonly double _minRttMs = double.MaxValue;
        private readonly DateTime _lastSendTime = DateTime.MinValue;
        private ISession _session;
        private int _receivedSize = 0;

        public void Dispose()
        {
            Monitor.Instance.SetMaxRttMs(_maxRttMs);
            Monitor.Instance.SetMinRttMs(_minRttMs);
            Monitor.Instance.AddClientCount(1);
            Monitor.Instance.AddTotalBytes(_totalBytes);
        }

        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            if (packet is Packet sendPacket == false)
            {
                throw new InvalidCastException(nameof(packet));
            }
            return sendPacket.Body;
        }

        public void SetSession(ISession session)
        {
            _session = session;
        }

        public Task OnReceivedAsync(ISession session, ArrayQueue<byte> buffer)
        {
            var count = buffer.Count;
            _receivedSize += count;
            buffer.Advance(count);

            while (_receivedSize >= Consts.Message.Length)
            {
                _ = session.SendAsync(Consts.Message);

                _receivedSize -= Consts.Message.Length;
            }
            _totalBytes += count;
            return Task.CompletedTask;
        }
    }
}
