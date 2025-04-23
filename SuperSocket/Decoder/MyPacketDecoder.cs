using SuperSocket.ProtoBase;
using SuperSocketServer.Models;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketServer.Decoder
{
    class MyPacketDecoder : IPackageDecoder<byte[]>
    {
        public byte[] Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            return buffer.ToArray();
            //var span = buffer.ToArray().AsSpan();
            //var opcode = span[0];
            //var payload = span.Slice(1).ToArray();
            //return new MyPacket
            //{
            //    Opcode = opcode,
            //    Payload = payload
            //};
        }
    }
}
