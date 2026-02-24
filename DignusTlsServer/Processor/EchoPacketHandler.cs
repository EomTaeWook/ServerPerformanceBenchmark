using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using DignusTlsServer.Packets;

namespace DignusTlsServer.Processor
{
    internal class EchoPacketHandler() : Dignus.Sockets.Processing.PacketProcessor, IPacketSerializer
    {
        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            if (packet is Packet sendPacket == false)
            {
                throw new InvalidCastException(nameof(packet));
            }
            return sendPacket.Body;
        }
        protected override Task ProcessPacketAsync(ISession session, ArraySegment<byte> packet)
        {
            var result = session.SendAsync(packet);
            if (result != SendResult.Success)
            {
                LogHelper.Error($"{result}");
            }
            return Task.CompletedTask;
        }

        protected override bool TakeReceivedPacket(ISession session, ArrayQueue<byte> buffer, out ArraySegment<byte> packet, out int consumedBytes)
        {
            var count = buffer.Count;
            consumedBytes = 0;
            if (!buffer.TrySlice(out packet, count))
            {
                return false;
            }
            consumedBytes = count;
            return true;
        }
    }
}
