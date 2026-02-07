using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Tls;
using DignusTlsClient.Serializer;
using System.Security.Cryptography.X509Certificates;

namespace DignusTlsClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LogBuilder.Configuration(LogConfigXmlReader.Load($"{AppContext.BaseDirectory}DignusLog.config"));
            LogBuilder.Build();
            
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Bechmark(1);
            Console.ReadLine();

        }

        private static void Bechmark(int clientCount)
        {
            var clients = new List<ClientModule>();
            LogHelper.Info($"start");

            var sessionConfiguration = new SessionConfiguration(EchoSetupFactory);

            sessionConfiguration.SocketOption.SendBufferSize = 65536;
            sessionConfiguration.SocketOption.MaxPendingSendBytes = int.MaxValue;

            {
                var pfxPath = Path.Combine(AppContext.BaseDirectory, "client.pfx");

                X509Certificate2 clientCert = X509CertificateLoader.LoadPkcs12FromFile(pfxPath, "1234");

                var tlsOption = new TlsClientOptions("localhost", clientCert);

                for(int i=0; i< clientCount; ++i )
                {
                    var client = new ClientModule(sessionConfiguration, tlsOption);
                    clients.Add(client);
                }

                try
                {
                    Parallel.For(0, clients.Count, (index) =>
                    {
                        clients[index].Connect("127.0.0.1", 5000);
                    });                                        
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }

            Monitor.Instance.Start();
            Task.Delay(10000).GetAwaiter().GetResult();
            {
                foreach (var client in clients)
                {
                    client.Close();
                }
                Monitor.Instance.Stop();
                Monitor.Instance.PrintEchoReport(Consts.Message.Length);
            }

        }
        private static SessionSetup EchoSetupFactory()
        {
            EchoPacketHandler echoSerializer = new();

            return new SessionSetup(
                    echoSerializer,
                    echoSerializer,
                    [echoSerializer]);
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Error(e.ExceptionObject as Exception);
        }
    }
}

