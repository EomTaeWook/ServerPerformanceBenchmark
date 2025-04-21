using DotNetty.Buffers;
using DotNetty.Transport.Channels;

namespace DotNettyServer.Handler
{
    internal class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext ctx, object msg)
        {
            var buffer = (IByteBuffer)msg;
            ctx.WriteAndFlushAsync(buffer.Retain()); // Echo
        }
    }
}
