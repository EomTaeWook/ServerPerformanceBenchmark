using Dignus.Sockets.Interfaces;

namespace EchoClient.Extensions
{
    internal static class SessionExtensions
    {
        public async static Task SendAsync(this ISession session, byte[] data)
        {
            if (session.GetSocket() == null)
            {
                return;
            }

            await Task.Yield();
            var sendResult = session.Send(data);
            if (sendResult != Dignus.Sockets.SendResult.Success)
            {
                Console.WriteLine($"failed to send : {sendResult}");
                session.Send(data);
            }
        }
    }
}
