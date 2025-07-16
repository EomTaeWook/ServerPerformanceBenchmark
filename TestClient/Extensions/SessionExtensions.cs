﻿using Dignus.Sockets.Interfaces;

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
                if (session.Send(data) != Dignus.Sockets.SendResult.Success)
                {
                    //Console.WriteLine("failed to send");
                    session.SendAsync(data);
                }
            }, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }
}
