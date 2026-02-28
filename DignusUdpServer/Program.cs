using Dignus.Log;
using Dignus.Sockets.Udp;
using DignusUdpServer;
using DignusUdpServer.Processor;
using System.Security.Cryptography.X509Certificates;

internal class Program
{
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        LogHelper.Error(e.ExceptionObject as Exception);
    }
    static void Main(string[] args)
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        LogBuilder.Configuration(LogConfigXmlReader.Load($"{AppContext.BaseDirectory}DignusLog.config"));
        LogBuilder.Build();

        //Echo
        var sessionConfiguration = new UdpSessionConfiguration(EchoSetupFactory);

        sessionConfiguration.SocketOption.SendBufferSize = 65536;
        sessionConfiguration.SocketOption.MaxPendingSendPackets = 65536;

        var serverOption = new UdpServerOption();

        EchoUdpServer echoServer = new(sessionConfiguration, serverOption);
        echoServer.Start(5000);
        LogHelper.Info($"start udp server... port : {5000}");
        Console.ReadKey();
    }
    static UdpSessionSetup EchoSetupFactory()
    {
        EchoPacketProcessor echoPacketProcessor = new();
        return new UdpSessionSetup(
                echoPacketProcessor,
                echoPacketProcessor,
                []);
    }
}