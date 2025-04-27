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
        static Tuple<IPacketSerializer, IPacketProcessor, ICollection<ISessionComponent>> SessionSetupFactory()
        {
            EchoHandler handler = new();

            PacketSerializer packetSerializer = new(handler);

            return Tuple.Create<IPacketSerializer, IPacketProcessor, ICollection<ISessionComponent>>(
                    packetSerializer,
                    packetSerializer,
                    [handler]);
        }
        static Tuple<IPacketSerializer, IPacketProcessor, ICollection<ISessionComponent>> EchoSetupFactory()
        {
            EchoSerializer echoSerializer = new();

            return Tuple.Create<IPacketSerializer, IPacketProcessor, ICollection<ISessionComponent>>(
                    echoSerializer,
                    echoSerializer,
                    [echoSerializer]);
        }

        public static byte[] Message = new byte[32];

        private static void SingleBechmark()
        {
            var clients = new List<ClientModule>();

            ThreadPool.GetMinThreads(out int workerThreads, out int ioThreads);
            ThreadPool.SetMinThreads(workerThreads, ioThreads + 20);

            Parallel.For(0, 1, (i) =>
            {
                var client = new ClientModule(new SessionConfiguration(EchoSetupFactory));

                try
                {
                    client.Connect("127.0.0.1", 5000);
                    client.SendMessage(Message, 1000);
                    clients.Add(client);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            });

            LogHelper.Info($"{clients.Count} clients connect complete");
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
                var client = new ClientModule(new SessionConfiguration(SessionSetupFactory));

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

            ProtocolHandlerMapper<EchoHandler, string>.BindProtocol<SCProtocol>();
            SingleBechmark();
            //ServerBechmark();

            Console.ReadLine();
        }
    }
}