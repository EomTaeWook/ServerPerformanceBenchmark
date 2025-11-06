using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using DignusEchoServer.Packets;

namespace DignusEchoServer.Serializer
{
    internal class EchoPacketHandler() : IPacketHandler, IPacketSerializer
    {
        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            if (packet is Packet sendPacket == false)
            {
                throw new InvalidCastException(nameof(packet));
            }
            return sendPacket.Body;
        }
        public Task OnReceivedAsync(ISession session, ArrayQueue<byte> buffer)
        {
            var count = buffer.Count;
            if (!buffer.TrySlice(out var packet, count))
            {
                return Task.CompletedTask;
            }
            buffer.Advance(count);
            var result = session.Send(packet);
            if (result != SendResult.Success)
            {
                LogHelper.Error($"{result}");
            }
            return Task.CompletedTask;
        }
    }
}
