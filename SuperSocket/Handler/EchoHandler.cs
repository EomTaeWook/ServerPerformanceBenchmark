using SuperSocket.Server.Abstractions.Session;
using SuperSocketServer.Packets;
using SuperSocketServer.Protocol;
using System.Text;
using System.Text.Json;

namespace SuperSocketServer.Handler
{
    internal class EchoHandler
    {
        public ValueTask Handle(IAppSession session, PacketInfo package)
        {
            if (package.Protocol == 0)
            {
                var bodyString = Encoding.UTF8.GetString(package.Body);
                Process(session, JsonSerializer.Deserialize<EchoMessage>(bodyString));
            }

            return ValueTask.CompletedTask;
        }
        private void Process(IAppSession session, EchoMessage echoMessage)
        {
            var body = JsonSerializer.Serialize(echoMessage);
            var packet = new Packet(0, body);
            _ = session.SendAsync(packet.ToBytes());
        }
    }
}
