using Dignus.Collections;
using Dignus.Log;
using Dignus.Sockets.Interfaces;
using System.Diagnostics;
using System.Text;

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

        public bool IsCompletePacketInBuffer(ArrayQueue<byte> buffer)
        {
            return true;
        }

        public void Deserialize(ArrayQueue<byte> buffer)
        {
            var bytes = buffer.Read(buffer.Count);
            var str = Encoding.UTF8.GetString(bytes);
            _sw.Stop();
            LogHelper.Info($"RTT : {_sw.ElapsedMilliseconds}");
            _sw.Restart();
            if (_session == null)
            {
                return;
            }
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
