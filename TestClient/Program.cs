// See https://aka.ms/new-console-template for more information
using Dignus.Extensions.Log;
using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using System.Collections.Concurrent;
using TestClient;
using TestClient.PacketSerializer;

internal class Program
{
    private static void Main(string[] args)
    {
        LogBuilder.Configuration(LogConfigXmlReader.Load("DignusLog.config")).Build();
        RunConnectClients();
    }
    static void RunConnectClients(int clientCount = 1000)
    {
        var clients = new ConcurrentBag<SocketClient>();
        Parallel.For(0, clientCount, (i) =>
        {
            try
            {
                var client = new SocketClient(new SessionConfiguration(SerializerFactory));
                clients.Add(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {i}번째 연결 실패: {ex.Message}");
            }
        });

        for (int i = 0; i < 100; ++i)
        {
            Parallel.ForEach(clients, (client) =>
            {
                client.Connect("127.0.0.1", 5000);
            });
            Thread.Sleep(1000);

            Parallel.ForEach(clients, (client) =>
            {
                client.Close();
            });
        }
    }
    static void RunClients(int clientCount = 1000)
    {
        var clients = new ConcurrentBag<SocketClient>();
        Parallel.For(0, clientCount, (i) =>
        {
            try
            {
                var client = new SocketClient(new SessionConfiguration(SerializerFactory));
                client.Connect("127.0.0.1", 5000);
                clients.Add(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {i}번째 연결 실패: {ex.Message}");
            }

        });
        Console.WriteLine($"{clients.Count} clients connected successfully.");
        Thread.Sleep(5000);
        foreach (var client in clients)
        {
            if (client == null)
            {
                return;
            }
            client.Close();
        }
    }
    static Tuple<IPacketSerializer, IPacketDeserializer, ICollection<ISessionComponent>> SerializerFactory()
    {
        var dummyPacketSerializer = new DummyPacketSerializer();
        var sessionComponents = new List<ISessionComponent>()
        {
            dummyPacketSerializer
        };

        return Tuple.Create<IPacketSerializer, IPacketDeserializer, ICollection<ISessionComponent>>(dummyPacketSerializer, dummyPacketSerializer, sessionComponents);
    }
}

