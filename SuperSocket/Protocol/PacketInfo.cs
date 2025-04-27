using System.Buffers;

namespace SuperSocketServer.Protocol
{
    internal class PacketInfo
    {
        public int Protocol { get; set; }

        public ReadOnlySequence<byte> Body { get; set; }
    }
}
