using Dignus.Log;
using Dignus.Sockets;
using DignusEchoServer.Handler;
using DignusEchoServer.Serializer;

namespace DignusEchoServer
{
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

            var sessionInitializer = new SessionConfiguration(EchoSetupFactory);
            sessionInitializer.SocketOption.SendBufferSize = 65536;
            sessionInitializer.SocketOption.MaxPendingSendBytes = int.MaxValue;
            EchoServer echoServer = new(sessionInitializer);
            echoServer.Start(5000);
            LogHelper.Info($"start server... port : {5000}");
            Console.ReadKey();
        }
        static SessionSetup EchoSetupFactory()
        {
            EchoPacketHandler echoPacketHandler = new();
            return new SessionSetup(
                    echoPacketHandler,
                    echoPacketHandler,
                    []);
        }
        static SessionSetup PacketHandlerSetupFactory()
        {
            EchoHandler handler = new();

            PacketSerializer packetSerializer = new(handler);

            return new SessionSetup(
                    packetSerializer,
                    packetSerializer,
                    [handler]);
        }
    }
}