using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketServer.Models
{
    public class MyPacket
    {
        public byte Opcode { get; set; }
        public byte[] Payload { get; set; }
    }

}
