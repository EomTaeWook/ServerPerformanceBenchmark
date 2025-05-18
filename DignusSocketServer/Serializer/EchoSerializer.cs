using Dignus.Collections;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using DignusEchoServer.Packets;

namespace DignusEchoServer.Serializer
{
    internal class EchoSerializer() : ISessionReceiver, IPacketSerializer
    {
        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            if (packet is Packet sendPacket == false)
            {
                throw new InvalidCastException(nameof(packet));
            }
            return sendPacket.Body;
        }

        public void OnReceived(ISession session, ArrayQueue<byte> buffer)
        {
            var count = buffer.Count;
            if (buffer.TrySlice(out var packet, count) == false)
            {
                return;
            }
            if (session.TrySend(packet) == false)
            {
                Console.WriteLine("failed to send");
            }
            buffer.Advance(packet.Count);
        }
    }
}
