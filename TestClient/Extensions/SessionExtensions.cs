using Dignus.Sockets.Interfaces;

namespace EchoClient.Extensions
{
    internal static class SessionExtensions
    {
        public static Task SendAsync(this ISession session, byte[] data)
        {
            return Task.Factory.StartNew(() =>
            {
                if (session.GetSocket() == null)
                {
                    return;
                }
                if (session.TrySend(data) == false)
                {
                    Console.WriteLine("failed to send");
                    session.SendAsync(data);
                }
            }, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }
}
