using Dignus.Log;
using Dignus.Sockets;
using EchoClient.Handler;
using EchoClient.Protocol;
using EchoClient.Serializer;

namespace EchoClient
{
    internal class Program
    {
        static SessionSetup PakcetHandlerSetupFactory()
        {
            EchoHandler handler = new();

            PacketSerializer packetSerializer = new(handler);

            return new SessionSetup(
                    packetSerializer,
                    packetSerializer,
                    [handler]);
        }
        static SessionSetup EchoSetupFactory()
        {
            EchoPacketProcessor echoPacketProcessor = new();

            return new SessionSetup(
                    echoPacketProcessor,
                    echoPacketProcessor,
                    []);
        }

        private static void SingleBechmark()
        {
            var clients = new List<ClientModule>();
            LogHelper.Info($"start");

            var sessionConfiguration = new SessionConfiguration(EchoSetupFactory);

            sessionConfiguration.SocketOption.SendBufferSize = 65536;
            sessionConfiguration.SocketOption.MaxPendingSendBytes = int.MaxValue;

            {
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
            }

            Monitor.Instance.Start();
            Task.Delay(10000).GetAwaiter().GetResult();

            {
                foreach (var client in clients)
                {
                    client.Close();
                }
                Monitor.Instance.PrintEcho("DignusSocketServer");
            }

        }

        private static void ServerBechmark(string serverName, int clientCount, int durationSec)
        {
            ProtocolHandlerMapper<EchoHandler, string>.BindProtocol<SCProtocol>();
            var clients = new List<ClientModule>();

            for (var i = 0; i < clientCount; ++i)
            {
                var sessionConfiguration = new SessionConfiguration(PakcetHandlerSetupFactory);
                sessionConfiguration.SocketOption.NoDelay = true; // same client config for every server
                var client = new ClientModule(sessionConfiguration);

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

            Task.Delay(durationSec * 1000).GetAwaiter().GetResult();

            foreach (var client in clients)
            {
                client.Close();
            }
            Monitor.Instance.Print(serverName);
        }

        static void Main(string[] args)
        {
            LogBuilder.Configuration(LogConfigXmlReader.Load($"{AppContext.BaseDirectory}DignusLog.config"));
            LogBuilder.Build();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var serverName = args.Length > 0 ? args[0] : "Server";
            var clientCount = 5000;
            var durationSec = 15;

            ServerBechmark(serverName, clientCount, durationSec);

            Console.ReadKey();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Error(e.ExceptionObject as Exception);
        }
    }
}