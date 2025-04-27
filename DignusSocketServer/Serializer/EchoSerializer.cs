using Dignus.Collections;
using Dignus.Sockets.Interfaces;
using DignusEchoServer.Packets;

namespace DignusEchoServer.Serializer
{
    internal class EchoSerializer() : IPacketProcessor, IPacketSerializer
    {
        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            if (packet is Packet sendPacket == false)
            {
                throw new InvalidCastException(nameof(packet));
            }
            return sendPacket.Body;
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

        public void ProcessPacket(ISession session, in ArraySegment<byte> packet)
        {
            session.Send(packet.Array);
        }
    }
}
