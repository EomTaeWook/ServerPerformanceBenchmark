using Dignus.Collections;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using Dignus.Sockets.Processing;
using DignusEchoServer.Handler;
using DignusEchoServer.Packets;
using System.Text;

namespace DignusEchoServer.Serializer
{
    internal class PacketSerializer(EchoHandler echoHandler) : SessionPacketProcessorBase, IPacketSerializer
    {
        private const int HeaderSize = sizeof(int) * 2;
        private const int SizeToInt = sizeof(int);

        public override void ProcessPacket(in ArraySegment<byte> packet)
        {
            var protocol = BitConverter.ToInt32(packet.Array, packet.Offset);
            var size = packet.Array.Length - packet.Offset - SizeToInt;
            var bodyString = Encoding.UTF8.GetString(packet.Array, packet.Offset + SizeToInt, size);
            ProtocolHandlerMapper<EchoHandler, string>.DispatchProtocolAction(echoHandler, protocol, bodyString);
        }

        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            if (packet is Packet sendPacket == false)
            {
                throw new InvalidOperationException("Invalid packet type");
            }
            var packetSize = sendPacket.GetLength();
            byte[] sendBuffer = new byte[packetSize + SizeToInt];

            Buffer.BlockCopy(BitConverter.GetBytes(packetSize), 0, sendBuffer, 0, SizeToInt);
            Buffer.BlockCopy(BitConverter.GetBytes(sendPacket.Protocol), 0, sendBuffer, SizeToInt, SizeToInt);
            Buffer.BlockCopy(sendPacket.Body, 0, sendBuffer, HeaderSize, sendPacket.Body.Length);

            return sendBuffer;
        }
        public override bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet, out int consumedBytes)
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
