using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using EchoClient.Packets;

namespace EchoClient.Serializer
{
    internal class EchoPacketHandler() : IPacketHandler, IPacketSerializer, ISessionComponent
    {
        private long _totalBytes = 0;
        private readonly double _maxRttMs = -1;
        private readonly double _minRttMs = double.MaxValue;
        private readonly DateTime _lastSendTime = DateTime.MinValue;
        private ISession _session;

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
            while (buffer.Count >= Consts.Message.Length)
            {
                if (!buffer.TrySlice(out ArraySegment<byte> segment, Consts.Message.Length))
                {
                    break;
                }
                buffer.Advance(segment.Count);
                var result = session.SendAsync(segment);
                if (result != SendResult.Success)
                {
                    LogHelper.Error($"{result}");
                    continue;
                }
                _totalBytes += Consts.Message.Length;
            }
            return Task.CompletedTask;
        }
    }
}
