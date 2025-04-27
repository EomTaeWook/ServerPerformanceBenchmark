using Dignus.Collections;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using DignusEchoServer.Handler;
using DignusEchoServer.Packets;
using System.Text;

namespace DignusEchoServer.Serializer
{
    internal class PacketSerializer(EchoHandler echoHandler) : IPacketDeserializer, IPacketSerializer
    {
        private const int SizeToInt = sizeof(int);
        public void Deserialize(in ArraySegment<byte> packet)
        {
            var protocol = BitConverter.ToInt32(packet.Array, 0);

            var bodyString = Encoding.UTF8.GetString(packet.Array, 4, packet.Array.Length - 4);

            ProtocolHandlerMapper<EchoHandler, string>.DispatchProtocolAction(echoHandler, protocol, bodyString);
        }

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
    }
}
