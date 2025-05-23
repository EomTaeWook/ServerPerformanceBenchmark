using Dignus.Collections;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using EchoClient.Extensions;
using EchoClient.Packets;
using System.Net.Sockets;

namespace EchoClient.Serializer
{
    internal class EchoSerializer() : ISessionPacketProcessor, IPacketSerializer, ISessionComponent
    {
        private long _totalBytes = 0;
        private double _maxRttMs = -1;
        private double _minRttMs = double.MaxValue;
        private DateTime _lastSendTime = DateTime.MinValue;
        private ISession _session;
        private int _receivedSize = 0;

        public void Dispose()
        {
            Monitor.Instance.SetMaxRttMs(_maxRttMs);
            Monitor.Instance.SetMinRttMs(_minRttMs);
            Monitor.Instance.AddClientCount(1);
            Monitor.Instance.AddTotalBytes(_totalBytes);
        }

        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            if (packet is Packet sendPacket == false)
            {
                throw new InvalidCastException(nameof(packet));
            }
            return sendPacket.Body;
        }

        public void SetSession(ISession session)
        {
            _session = session;
        }

        public void OnReceived(ISession session, ArrayQueue<byte> buffer)
        {
            var count = buffer.Count;
            _receivedSize += count;
            buffer.Advance(count);

            while (_receivedSize >= Consts.Message.Length)
            {
                //try
                //{
                //    session.Send(Consts.Message);
                //}
                //catch (Exception ex)
                //{
                //    LogHelper.Error(ex);
                //}
                session.SendAsync(Consts.Message);
                _receivedSize -= Consts.Message.Length;
            }
            _totalBytes += count;
        }
    }
}
