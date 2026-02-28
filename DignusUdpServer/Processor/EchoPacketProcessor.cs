using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using DignusUdpServer.Packets;

namespace DignusUdpServer.Processor
{
    internal class EchoPacketProcessor() : Dignus.Sockets.Processing.UdpPacketProcessor, IPacketSerializer
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
    }
}
