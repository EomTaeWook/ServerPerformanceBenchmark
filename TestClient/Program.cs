using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using EchoClient.Handler;
using EchoClient.Protocol;
using EchoClient.Serializer;

namespace EchoClient
{
    internal class Program
    {
        static Tuple<IPacketSerializer, IPacketHandler, ICollection<ISessionComponent>> PakcetHandlerSetupFactory()
        {
            EchoHandler handler = new();

            PacketSerializer packetSerializer = new(handler);

            return Tuple.Create<IPacketSerializer, IPacketHandler, ICollection<ISessionComponent>>(
                    packetSerializer,
                    packetSerializer,
                    [handler]);
        }
        static Tuple<IPacketSerializer, IPacketHandler, ICollection<ISessionComponent>> EchoSetupFactory()
        {
            EchoSerializer echoSerializer = new();

            return Tuple.Create<IPacketSerializer, IPacketHandler, ICollection<ISessionComponent>>(
                    echoSerializer,
                    echoSerializer,
                    [echoSerializer]);
        }

        private static void SingleBechmark()
        {
            var clients = new List<ClientModule>();
            LogHelper.Info($"start");

            ThreadPool.GetMinThreads(out int workerThreads, out int ioThreads);
            ThreadPool.SetMinThreads(workerThreads, ioThreads + 100);

            Parallel.For(0, 1, (i) =>
            {
                var sessionConfiguration = new SessionConfiguration(EchoSetupFactory);

                sessionConfiguration.SocketOption.SendBufferSize = 65536 * 20;

                var client = new ClientModule(sessionConfiguration);

                try
                {
                    client.Connect("127.0.0.1", 5000);
                    clients.Add(client);
                    client.SendMessage(Consts.Message, 1000);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            });

            Monitor.Instance.Start();
            Task.Delay(10000).GetAwaiter().GetResult();
            foreach (var client in clients)
            {
                client.Close();
            }
            Monitor.Instance.PrintEcho("DignusSocketServer");
        }

        private static void ServerBechmark()
        {
            var clients = new List<ClientModule>();

            for (var i = 0; i < 5000; ++i)
            {
                var client = new ClientModule(new SessionConfiguration(PakcetHandlerSetupFactory));

                try
                {
                    client.Connect("127.0.0.1", 5000);
                    clients.Add(client);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }

            LogHelper.Info($"{clients.Count} clients connect complete");

            Parallel.ForEach(clients, client =>
            {
                client.SendEcho("Hello Dignus Socket");
            });

            Task.Delay(30000).GetAwaiter().GetResult();

            foreach (var client in clients)
            {
                client.Close();
            }
            Monitor.Instance.Print("DignusSocketServer");
        }
        static void Main(string[] args)
        {
            LogBuilder.Configuration(LogConfigXmlReader.Load($"{AppContext.BaseDirectory}DignusLog.config"));
            LogBuilder.Build();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ProtocolHandlerMapper<EchoHandler, string>.BindProtocol<SCProtocol>();
            //SingleBechmark();
            ServerBechmark();

            Console.ReadLine();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Error(e.ExceptionObject as Exception);
        }
    }
}