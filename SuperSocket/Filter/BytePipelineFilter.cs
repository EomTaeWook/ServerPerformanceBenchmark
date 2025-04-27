using SuperSocket.ProtoBase;
using SuperSocketServer.Protocol;
using System.Buffers;

namespace SuperSocketServer.Filter
{
    class BytePipelineFilter : FixedHeaderPipelineFilter<PacketInfo>
    {
        public BytePipelineFilter() : base(4)
        {
        }
        protected override PacketInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            var reader = new SequenceReader<byte>(buffer);

            if (reader.TryReadLittleEndian(out int bodySize) == false)
            {
                return default;
            }

            if (reader.TryReadLittleEndian(out int protocol) == false)
            {
                return default;
            }

            int bodyLength = bodySize - 4;

            if (reader.TryReadExact(bodyLength, out ReadOnlySequence<byte> body) == false)
            {
                return default;
            }
            return new PacketInfo()
            {
                Body = body,
                Protocol = protocol,
            };
        }
        protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
        {
            if (buffer.Length < 4)
            {
                return 0;
            }
            var reader = new SequenceReader<byte>(buffer);
            if (!reader.TryReadLittleEndian(out int bodySize))
            {
                return 0;
            }
            return bodySize;
        }
    }
}
