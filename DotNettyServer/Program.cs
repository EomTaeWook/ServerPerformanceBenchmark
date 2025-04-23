using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNettyServer.Handler;
using System.Net;

Console.WriteLine("Starting Echo Server...");
var bossGroup = new MultithreadEventLoopGroup(1);
var workerGroup = new MultithreadEventLoopGroup();

try
{
    var bootstrap = new ServerBootstrap();

    bootstrap
        .Group(bossGroup, workerGroup)
        .Channel<TcpServerSocketChannel>()
        .Option(ChannelOption.SoBacklog, 200)
        .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
        {
            IChannelPipeline pipeline = channel.Pipeline;
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


int gen0 = GC.CollectionCount(0);
int gen1 = GC.CollectionCount(1);
int gen2 = GC.CollectionCount(2);
int total = gen0 + gen1 + gen2;

Console.WriteLine($"Gen0: {gen0}, Gen1: {gen1}, Gen2: {gen2}, Total: {total}");