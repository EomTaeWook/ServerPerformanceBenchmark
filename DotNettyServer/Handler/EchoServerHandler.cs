using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNettyServer.Packets;
using System.Text;
using System.Text.Json;

namespace DotNettyServer.Handler
{
    internal class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {
            var buffer = (IByteBuffer)msg;
            if (buffer.ReadableBytes < 4)
            {
                ctx.FireChannelRead(msg);
                return;
            }

            var bodySize = buffer.GetIntLE(buffer.ReaderIndex);

            if (buffer.ReadableBytes - 4 < bodySize)
            {
                ctx.FireChannelRead(msg);
                return;
            }
            var protocol = buffer.GetIntLE(4);

            var body = buffer.GetString(8, bodySize - 4, Encoding.UTF8);

            buffer.Release();

            switch (protocol)
            {
                case 0:
                    Process(ctx, Deserialize<EchoMessage>(body));
                    break;
            }
        }

        private T Deserialize<T>(string jsonBody)
        {
            return JsonSerializer.Deserialize<T>(jsonBody);
        }

        private void Process(IChannelHandlerContext ctx, EchoMessage echoMessage)
        {
            var body = JsonSerializer.Serialize(echoMessage);
            var packet = new Packet(0, body);
            var buffer = packet.ToBytes();
            _ = ctx.WriteAndFlushAsync(buffer);
        }
    }
}
