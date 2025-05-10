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
        public void ProcessPacket(ISession session, in ArraySegment<byte> packet)
        {
            //Console.WriteLine($"session : {session.Id}, packet : {packet.Count} {count++}");
            //session.Send(packet);
            if (session.TrySend(packet) == false)
            {
                Console.WriteLine("failed to send");
            }
        }

        public bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet, out int consumedBytes)
        {
            consumedBytes = buffer.Count;
            return buffer.TrySlice(out packet, consumedBytes);
        }
        public bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet)
        {
            packet = null;
            if (buffer.TryReadBytes(out byte[] bytes, buffer.Count) == false)
            {
                return false;
            }
            packet = bytes;
            return true;
        }
    }
}
