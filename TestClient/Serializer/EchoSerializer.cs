using Dignus.Collections;
using Dignus.Sockets.Interfaces;
using EchoClient.Packets;

namespace EchoClient.Serializer
{
    internal class EchoSerializer() : IPacketDeserializer, IPacketSerializer, ISessionComponent
    {
        private ISession _session;

        private long _receivedCount;
        private long _totalBytes = 0;
        private double _maxRttMs = -1;
        private double _minRttMs = double.MaxValue;
        private DateTime _lastSendTime = DateTime.MinValue;

        private int _receivedSize = 0;

        public void Deserialize(in ArraySegment<byte> packet)
        {
            _receivedSize += packet.Count;

            while (_receivedSize >= Program.Message.Length)
            {
                Task.Run(() =>
                {
                    _session.Send(Program.Message);
                });

                _receivedSize -= Program.Message.Length;
            }
            Interlocked.Add(ref _totalBytes, packet.Count);
            //var rtt = (DateTime.UtcNow - _lastSendTime).TotalMilliseconds;
            //if (rtt > _maxRttMs)
            //    _maxRttMs = rtt;
            //if (rtt < _minRttMs)
            //    _minRttMs = rtt;
            _lastSendTime = DateTime.UtcNow;
        }
        public void SendMessage(in ArraySegment<byte> packet)
        {
            _lastSendTime = DateTime.UtcNow;
            _session.Send(packet);
        }
        public void Dispose()
        {
            Monitor.Instance.AddReceivedCount(_receivedCount);
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

        public bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet)
        {
            packet = null;
            if (buffer.TryRead(out byte[] bytes, buffer.Count) == false)
            {
                return false;
            }
            packet = bytes;
            return true;
        }
    }
}
