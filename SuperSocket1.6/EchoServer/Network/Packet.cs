using Google.Protobuf;
using System;

namespace EchoServer.Network
{
	public partial class Packet
	{
		public byte[] Body { get; private set; } = Array.Empty<byte>();
		public Packet(Proto.S2C packet)
		{
			Body = packet.ToByteArray();
		}

		public int GetLength()
		{
			return Body.Length;
		}

		public ArraySegment<byte> ToByteArrayWithLengthPrefix()
		{
			var length = BitConverter.GetBytes(Body.Length);
			var buffer = new byte[4 + Body.Length];

			Buffer.BlockCopy(length, 0, buffer, 0, 4);
			Buffer.BlockCopy(Body, 0, buffer, 4, Body.Length);

			return new ArraySegment<byte>(buffer);
		}
	}
}
