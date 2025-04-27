using System.Text;

namespace SuperSocketServer.Packets
{
    internal class Packet
    {
        public int Protocol { get; }
        public byte[] Data { get; }
        public Packet(int protocol, string body)
        {
            Protocol = protocol;
            Data = Encoding.UTF8.GetBytes(body);
        }

        public byte[] ToBytes()
        {
            var packetBytes = new byte[8 + Data.Length];

            Buffer.BlockCopy(BitConverter.GetBytes(packetBytes.Length - 4), 0, packetBytes, 0, 4);

            Buffer.BlockCopy(BitConverter.GetBytes(Protocol), 0, packetBytes, 4, 4);

            Buffer.BlockCopy(Data, 0, packetBytes, 8, Data.Length);

            return packetBytes;
        }
    }
}
