using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;
using SuperSocketServer.Filter;
using SuperSocketServer.Handler;
using SuperSocketServer.Protocol;

internal class Program
{
    private static void Main(string[] args)
    {
        var echoHandler = new EchoHandler();
        var host = SuperSocketHostBuilder.Create<PacketInfo, BytePipelineFilter>()
            .UsePackageHandler((async (s, p) =>
            {
                await echoHandler.Handle(s, p);
            }));

        host.ConfigureSuperSocket(options =>
        {
            options.AddListener(new ListenOptions
            {
                Ip = "Any",
                Port = 5000,
                BackLog = 200,
                NoDelay = true,
            });
        }).ConfigureLogging((logging) =>
        {
            logging.ClearProviders();
        })
        .Build()
        .RunAsync();


        Console.WriteLine($"Start Echo Server");
        Console.ReadLine();

        int gen0 = GC.CollectionCount(0);
        int gen1 = GC.CollectionCount(1);
        int gen2 = GC.CollectionCount(2);
        int total = gen0 + gen1 + gen2;

        Console.WriteLine($"Gen0: {gen0}, Gen1: {gen1}, Gen2: {gen2}, Total: {total}");
    }
}