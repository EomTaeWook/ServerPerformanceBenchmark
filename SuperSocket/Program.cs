using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;
using SuperSocketServer.Filter;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = SuperSocketHostBuilder.Create<byte[], BytePipelineFilter>()
            .UsePackageHandler((async (s, p) =>
            {
                await s.SendAsync(p.ToArray());
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
        .Run();

        int gen0 = GC.CollectionCount(0);
        int gen1 = GC.CollectionCount(1);
        int gen2 = GC.CollectionCount(2);
        int total = gen0 + gen1 + gen2;

        Console.WriteLine($"Gen0: {gen0}, Gen1: {gen1}, Gen2: {gen2}, Total: {total}");
    }
}