using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using DignusEchoServer.Handler;
using DignusEchoServer.Serializer;

namespace DignusEchoServer
{
    internal class Program
    {

        static void Main(string[] args)
        {
            LogBuilder.Configuration(LogConfigXmlReader.Load($"{AppContext.BaseDirectory}DignusLog.config"));
            LogBuilder.Build();

            var sessionInitializer = new SessionConfiguration(EchoSetupFactory);

            EchoServer echoServer = new(sessionInitializer);
            echoServer.Start(5000);
            LogHelper.Info($"start server... port : {5000}");
            Console.ReadKey();
        }
        static Tuple<IPacketSerializer, IPacketProcessor, ICollection<ISessionComponent>> EchoSetupFactory()
        {
            EchoSerializer echoSerializer = new();
            return Tuple.Create<IPacketSerializer, IPacketProcessor, ICollection<ISessionComponent>>(
                    echoSerializer,
                    echoSerializer,
                    []);
        }
        static Tuple<IPacketSerializer, IPacketProcessor, ICollection<ISessionComponent>> SessionSetupFactory()
        {
            EchoHandler handler = new();

            PacketSerializer packetSerializer = new(handler);

            return Tuple.Create<IPacketSerializer, IPacketProcessor, ICollection<ISessionComponent>>(
                    packetSerializer,
                    packetSerializer,
                    [handler]);
        }
    }
}