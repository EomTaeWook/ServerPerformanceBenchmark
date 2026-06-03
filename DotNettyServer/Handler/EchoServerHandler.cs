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
            // LengthFieldBasedFrameDecoder guarantees msg is exactly one complete frame
            // starting at offset 0: [size:int32 LE][protocol:int32 LE][json].
            var buffer = (IByteBuffer)msg;
            try
            {
                var bodySize = buffer.GetIntLE(0);
                var protocol = buffer.GetIntLE(4);
                var body = buffer.GetString(8, bodySize - 4, Encoding.UTF8);

                if (protocol == 0)
                {
                    Process(ctx, Deserialize<EchoMessage>(body));
                }
            }
            finally
            {
                buffer.Release();
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
