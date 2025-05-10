using Dignus.Collections;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using DignusEchoServer.Handler;
using DignusEchoServer.Packets;
using System.Text;

namespace DignusEchoServer.Serializer
{
    internal class PacketSerializer(EchoHandler echoHandler) : IPacketProcessor, IPacketSerializer
    {
        private const int SizeToInt = sizeof(int);

        public void ProcessPacket(ISession session, in ArraySegment<byte> packet)
        {
            var protocol = BitConverter.ToInt32(packet.Array, 0);

            var bodyString = Encoding.UTF8.GetString(packet.Array, 4, packet.Array.Length - 4);

            ProtocolHandlerMapper<EchoHandler, string>.DispatchProtocolAction(echoHandler, protocol, bodyString);
        }

        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            if (packet is Packet sendPacket == false)
            {
                throw new InvalidOperationException("Invalid packet type");
            }

            byte[] bytes = new byte[sendPacket.GetLength() + sizeof(int)];
            var size = sendPacket.GetLength();
            Buffer.BlockCopy(BitConverter.GetBytes(size), 0, bytes, 0, 4);
            Buffer.BlockCopy(sendPacket.Body, 0, bytes, 4, size);

            return bytes;
        }
        public bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet)
        {
            packet = null;
            if (buffer.Count < SizeToInt)
            {
                return false;
            }

            var headerBytes = buffer.Peek(SizeToInt);

            var bodySize = BitConverter.ToInt32(headerBytes);

            if (buffer.Count < bodySize + SizeToInt)
            {
                return false;
            }

            if (buffer.TryRead(out _, SizeToInt) == false)
            {
                return false;
            }

            if (buffer.TryRead(out byte[] bodyBytes, bodySize) == false)
            {
                return false;
            }

            packet = bodyBytes;

            return true;
        }
        public bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet, out int consumedBytes)
        {
            packet = null;
            consumedBytes = 0;
            if (buffer.Count < SizeToInt)
            {
                return false;
            }

            var headerBytes = buffer.Peek(SizeToInt);

            var bodySize = BitConverter.ToInt32(headerBytes);

            if (buffer.Count < bodySize + SizeToInt)
            {
                return false;
            }

            if (buffer.TryReadBytes(out _, SizeToInt) == false)
            {
                return false;
            }

            if (buffer.TrySlice(out packet, bodySize) == false)
            {
                return false;
            }

            consumedBytes = bodySize;

            return true;
        }
    }
}
