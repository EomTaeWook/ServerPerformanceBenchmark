using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Session;

namespace SuperSocketServer.Handler
{
    public class EchoHandler : IPackageHandler<byte[]>
    {
        public async ValueTask Handle(IAppSession session, byte[] package, CancellationToken cancellationToken)
        {
            await session.SendAsync(package);
        }
    }
}
