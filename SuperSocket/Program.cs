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
            // Tuned: larger accept backlog for high connection counts (default socket buffers).
            options.AddListener(new ListenOptions
            {
                Ip = "Any",
                Port = 5000,
                BackLog = 1024,
                NoDelay = true,
            });
        }).ConfigureLogging((logging) =>
        {
            logging.ClearProviders();
        });

        Console.WriteLine($"Start Echo Server");
        // Headless: block until the process is killed (no interactive stdin in benchmark harness).
        host.Build().Run();

        Console.ReadKey();
    }
}