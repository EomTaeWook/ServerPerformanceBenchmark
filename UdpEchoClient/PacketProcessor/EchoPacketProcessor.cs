using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Interfaces;
using Dignus.Sockets.Processing;
using System;
using System.Threading.Tasks;

namespace UdpEchoClient.PacketProcessor
{
    internal class EchoPacketProcessor : UdpPacketProcessor, ISessionComponent, IPacketSerializer
    {
        private long _totalBytes = 0;
        private readonly double _maxRttMs = -1;
        private readonly double _minRttMs = double.MaxValue;

        protected override Task ProcessPacketAsync(ISession session, ArraySegment<byte> packet)
        {
            var result = session.SendAsync(packet);
            if (result != SendResult.Success && result != SendResult.Disposed)
            {
                LogHelper.Error($"{result}");
            }
            _totalBytes += Consts.Message.Length;

            return Task.CompletedTask;
        }

        public void SetSession(ISession session)
        {
            
        }

        public void Dispose()
        {
            Monitor.Instance.SetMaxRttMs(_maxRttMs);
            Monitor.Instance.SetMinRttMs(_minRttMs);
            Monitor.Instance.AddClientCount(1);
            Monitor.Instance.AddTotalBytes(_totalBytes);
        }

        public ArraySegment<byte> MakeSendBuffer(IPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}
