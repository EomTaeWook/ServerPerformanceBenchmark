using Dignus.Collections;
using Dignus.Sockets.Interfaces;

namespace IndividualServer.PacketSerializer
{
    internal class DummyPacketSerializer : IPacketSerializer, IPacketDeserializer, ISessionComponent
    {
        ISession _session;
        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            return null;
        }

        public bool IsCompletePacketInBuffer(ArrayQueue<byte> buffer)
        {
            return buffer.CanRead;
        }

        public void Deserialize(ArrayQueue<byte> buffer)
        {
            var bytes = buffer.Read(buffer.Count);
            _session.Send(bytes);
        }

        public void SetSession(ISession session)
        {
            _session = session;
        }

        public void Dispose()
        {
            _session = null;
        }
    }
}
