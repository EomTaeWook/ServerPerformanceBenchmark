using Dignus.Collections;
using Dignus.Sockets.Interfaces;
using System.Diagnostics;

namespace TestClient.PacketSerializer
{
    internal class DummyPacketSerializer : IPacketSerializer, IPacketDeserializer, ISessionComponent
    {
        ISession _session;
        Stopwatch _sw = new Stopwatch();
        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            return null;
        }

        public void SetSession(ISession session)
        {
            _session = session;
        }

        public void Dispose()
        {
            _session = null;
        }

        public bool TakeReceivedPacket(ArrayQueue<byte> buffer, out ArraySegment<byte> packet)
        {
            packet = buffer.Read(buffer.Count);

            return true;
        }

        public void Deserialize(in ArraySegment<byte> packet)
        {
            _session.Send(packet);
        }
    }
}
