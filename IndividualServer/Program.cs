using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using IndividualServer;
using IndividualServer.PacketSerializer;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting Echo Server...");

        var server = new Server(new SessionConfiguration(SerializerFactory, new SocketOption()
        {
            BufferSize = 4096,
            KeepAlive = false,
        }));

        server.Start(6000, 1000);
        Console.WriteLine("Echo Server started on port 6000.");

        Console.ReadLine();
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