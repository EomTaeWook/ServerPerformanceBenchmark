using Dignus.Collections;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using EchoClient.Extensions;
using EchoClient.Packets;

namespace EchoClient.Serializer
{
    internal class EchoSerializer() : IPacketProcessor, IPacketSerializer, ISessionComponent
    {
        private long _totalBytes = 0;
        private double _maxRttMs = -1;
        private double _minRttMs = double.MaxValue;
        private DateTime _lastSendTime = DateTime.MinValue;
        private ISession _session;
        private int _receivedSize = 0;

        private int _sendCount = 0;
        public void ProcessPacket(ISession session, in ArraySegment<byte> packet)
        {
            _receivedSize += packet.Count;
            while (_receivedSize >= Consts.Message.Length)
            {
                //try
                //{
                //    session.Send(Consts.Message);
                //}
                //catch (Exception ex)
                //{
                //    LogHelper.Error(ex);
                //}
                session.SendAsync(Consts.Message);
                _receivedSize -= Consts.Message.Length;
            }
            Interlocked.Add(ref _totalBytes, packet.Count);
        }
        public void Dispose()
        {
            Monitor.Instance.SetMaxRttMs(_maxRttMs);
            Monitor.Instance.SetMinRttMs(_minRttMs);
            Monitor.Instance.AddClientCount(1);
            Monitor.Instance.AddTotalBytes(_totalBytes);
            Monitor.Instance.AddTotalSendCount(_sendCount);
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

        public bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet, out int consumedBytes)
        {
            consumedBytes = buffer.Count;
            return buffer.TrySlice(out packet, consumedBytes);
        }

        public bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet)
        {
            if (buffer.TryReadBytes(out byte[] body, buffer.Count) == false)
            {
                packet = null;
                return false;
            }
            packet = body;
            return true;
        }
    }
}
