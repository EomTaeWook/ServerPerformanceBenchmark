using Dignus.Log;
using Dignus.Sockets.Udp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UdpEchoClient;
using UdpEchoClient.PacketProcessor;

internal class Program
{
    static UdpSessionSetup EchoSetupFactory()
    {
        EchoPacketProcessor echoPacketProcessor = new();

        return new UdpSessionSetup(
                echoPacketProcessor,
                echoPacketProcessor,
                [echoPacketProcessor]);
    }

    private static void RttBechmark()
    {
        var clients = new List<ClientModule>();
        LogHelper.Info($"start");

        var sessionConfiguration = new UdpSessionConfiguration(EchoSetupFactory);

        sessionConfiguration.SocketOption.SendBufferSize = 65536;
        sessionConfiguration.SocketOption.MaxPendingSendPackets = 65536;

        {
            var client = new ClientModule(sessionConfiguration);

            try
            {
                client.Connect("127.0.0.1", 5000);
                clients.Add(client);
                client.SendMessage(Consts.Message, 1000);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        Monitor.Instance.Start();
        Task.Delay(10000).GetAwaiter().GetResult();

        string ServerName = "DignusUdpSocketServer";

        {
            foreach (var client in clients)
            {
                client.Close();
            }
            Monitor.Instance.PrintEcho(ServerName);
        }

    }
    private static void Main(string[] args)
    {
        LogBuilder.Configuration(LogConfigXmlReader.Load($"{AppContext.BaseDirectory}DignusLog.config"));
        LogBuilder.Build();

        RttBechmark();

        Console.ReadLine();
    }
}