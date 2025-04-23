using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using IndividualServer;
using IndividualServer.PacketSerializer;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting Echo Server...");

        var server = new Server(new SessionConfiguration(SerializerFactory));

        server.Start(5000);
        Console.WriteLine("Echo Server started on port 5000.");

        Console.ReadLine();

        int gen0 = GC.CollectionCount(0);
        int gen1 = GC.CollectionCount(1);
        int gen2 = GC.CollectionCount(2);
        int total = gen0 + gen1 + gen2;

        Console.WriteLine($"Gen0: {gen0}, Gen1: {gen1}, Gen2: {gen2}, Total: {total}");
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