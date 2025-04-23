using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketServer.Filter
{
    class BytePipelineFilter : PipelineFilterBase<byte[]>
    {
        public override byte[] Filter(ref SequenceReader<byte> reader)
        {
            if(reader.Remaining > 0)
            {
                var bytes = reader.Sequence.ToArray();
                reader.Advance(reader.Remaining);
                return bytes;
            }
            return null; 
        }
    }
}
