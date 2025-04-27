using Dignus.Collections;
using Dignus.Sockets.Interfaces;
using DignusEchoServer.Packets;

namespace DignusEchoServer.Serializer
{
    internal class EchoSerializer() : IPacketDeserializer, IPacketSerializer, ISessionComponent
    {
        private ISession _session;
        private const int SizeToInt = sizeof(int);
        public void Deserialize(in ArraySegment<byte> packet)
        {
            _session.Send(packet.Array);
        }

        public void Dispose()
        {
            _session = null;
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
