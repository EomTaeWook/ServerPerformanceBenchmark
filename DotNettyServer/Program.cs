using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNettyServer.Handler;
using System.Net;

Console.WriteLine("Starting Dotnetty Echo Server...");
var bossGroup = new MultithreadEventLoopGroup();
var workerGroup = new MultithreadEventLoopGroup();

try
{
    var bootstrap = new ServerBootstrap();

    bootstrap
        .Group(bossGroup, workerGroup)
        .Channel<TcpServerSocketChannel>()
        .Option(ChannelOption.SoBacklog, 200)
        .ChildOption(ChannelOption.TcpNodelay, true)
        .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
        {
            IChannelPipeline pipeline = channel.Pipeline;
            // Idiomatic framing: [len:int32 LE] header. length value counts bytes after the field
            // (protocol + json). Deliver the full frame (initialBytesToStrip=0) so the handler reads
            // size@0 / protocol@4 / body@8 from a guaranteed-complete frame.
            pipeline.AddLast(new LengthFieldBasedFrameDecoder(ByteOrder.LittleEndian, int.MaxValue, 0, 4, 0, 0, true));
            pipeline.AddLast(new EchoServerHandler());
        }));

    IChannel boundChannel = await bootstrap.BindAsync(IPAddress.Loopback, 5000);
    Console.WriteLine("Echo Server started on port 5000.");

    await boundChannel.CloseCompletion;
}
finally
{
    await Task.WhenAll(
        bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
        workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1))
    );
}

Console.ReadKey();

int gen0 = GC.CollectionCount(0);
int gen1 = GC.CollectionCount(1);
int gen2 = GC.CollectionCount(2);
int total = gen0 + gen1 + gen2;

Console.WriteLine($"Gen0: {gen0}, Gen1: {gen1}, Gen2: {gen2}, Total: {total}");
