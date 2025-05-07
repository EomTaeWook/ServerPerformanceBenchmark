using Dignus.Collections;
using Dignus.Sockets;
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
            if (buffer.TryReadBytes(out byte[] bytes, buffer.Count))
            {
                packet = bytes;
                return true;
            }
            packet = null;
            return false;
        }
        private int count;
        public void ProcessPacket(ISession session, in ArraySegment<byte> packet)
        {
            //Console.WriteLine($"session : {session.Id}, packet : {packet.Count} {count++}");
            session.Send(packet);
        }
    }
}
