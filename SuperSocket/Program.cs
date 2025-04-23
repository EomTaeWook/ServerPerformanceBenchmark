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
    }
}