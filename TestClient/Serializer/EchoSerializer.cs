using Dignus.Collections;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using EchoClient.Packets;

namespace EchoClient.Serializer
{
    internal class EchoSerializer() : IPacketProcessor, IPacketSerializer, ISessionComponent
    {
        private long _receivedCount;
        private long _totalBytes = 0;
        private double _maxRttMs = -1;
        private double _minRttMs = double.MaxValue;
        private DateTime _lastSendTime = DateTime.MinValue;
        private ISession _session;
        private int _receivedSize = 0;
        public void ProcessPacket(ISession session, in ArraySegment<byte> packet)
        {
            _receivedSize += packet.Count;
            while (_receivedSize >= Consts.Message.Length)
            {
                //session.Send(Consts.Message);
                Task.Factory.StartNew(() =>
                {
                    session.Send(Consts.Message);

                }, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.RunContinuationsAsynchronously);
                _receivedSize -= Consts.Message.Length;
            }
            Interlocked.Add(ref _totalBytes, packet.Count);
        }
        public void Dispose()
        {
            _session.Send(Consts.Message);
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
