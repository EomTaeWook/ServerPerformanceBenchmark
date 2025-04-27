using Dignus.Collections;
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

        private int _receivedSize = 0;
        public void ProcessPacket(ISession session, in ArraySegment<byte> packet)
        {
            _receivedSize += packet.Count;
            while (_receivedSize >= Program.Message.Length)
            {
                Task.Factory.StartNew(() =>
                {
                    session.Send(Program.Message);
                }, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.RunContinuationsAsynchronously);
                _receivedSize -= Program.Message.Length;
            }
            Interlocked.Add(ref _totalBytes, packet.Count);
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
