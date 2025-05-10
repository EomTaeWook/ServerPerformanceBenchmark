using Dignus.Collections;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using EchoClient.Handler;
using EchoClient.Packets;
using System.Text;

namespace EchoClient.Serializer
{
    internal class PacketSerializer(EchoHandler echoHandler) : IPacketProcessor, IPacketSerializer
    {
        private const int SizeToInt = sizeof(int);

        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            if (packet is Packet sendPacket == false)
            {
                throw new InvalidCastException(nameof(packet));
            }

            byte[] buffer = new byte[SizeToInt + sendPacket.GetLength()];

            var bodySizeBytes = BitConverter.GetBytes(sendPacket.GetLength());

            Array.Copy(bodySizeBytes, buffer, SizeToInt);
            var protocolBytes = BitConverter.GetBytes(sendPacket.Protocol);

            Array.Copy(protocolBytes, 0, buffer, SizeToInt, SizeToInt);

            Array.Copy(sendPacket.Body, 0, buffer, SizeToInt + SizeToInt, sendPacket.Body.Length);

            return buffer;
        }

        public void ProcessPacket(ISession session, in ArraySegment<byte> packet)
        {
            var protocol = BitConverter.ToInt32(packet.Array, 0);

            var bodyString = Encoding.UTF8.GetString(packet.Array, 4, packet.Array.Length - 4);

            ProtocolHandlerMapper<EchoHandler, string>.DispatchProtocolAction(echoHandler, protocol, bodyString);
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

            if (buffer.TryReadBytes(out _, SizeToInt) == false)
            {
                return false;
            }

            if (buffer.TryReadBytes(out byte[] body, bodySize) == false)
            {
                return false;
            }
            packet = body;
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

            consumedBytes = BitConverter.ToInt32(headerBytes);

            if (buffer.Count < consumedBytes + SizeToInt)
            {
                consumedBytes = 0;
                return false;
            }

            if (buffer.TryReadBytes(out _, SizeToInt) == false)
            {
                consumedBytes = 0;
                return false;
            }

            if (buffer.TrySlice(out packet, consumedBytes) == false)
            {
                consumedBytes = 0;
                return false;
            }
            return true;
        }
    }
}
