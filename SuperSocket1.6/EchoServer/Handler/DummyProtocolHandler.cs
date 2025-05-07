using EchoServer.Network;
using Proto;
using System;
using System.Net.Sockets;

namespace EchoServer.Handler
{
    public class DummyProtocolHandler
    {
		public static void Process(NetworkSession session, LoginReq login)
		{
			var packet = new Packet(new S2C()
			{
				LoginRes = new LoginRes()
				{
					Success = true,
				}
			});

			session.Send(packet.ToByteArrayWithLengthPrefix());
		}

		public static void Process(NetworkSession session, JoinRoomReq joinRoomReq)
		{
			var packet = new Packet(new S2C()
			{
				JoinRoomRes = new JoinRoomRes()
				{
					Success = true,
				}
			});
			session.Send(packet.ToByteArrayWithLengthPrefix());
		}
	}
}
