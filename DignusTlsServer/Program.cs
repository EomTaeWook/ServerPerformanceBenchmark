using Dignus.Log;
using Dignus.Sockets;
using Dignus.Sockets.Tls;
using DignusTlsServer.Processor;
using System.Security.Cryptography.X509Certificates;

namespace DignusTlsServer
{
    internal class Program
    {
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Error(e.ExceptionObject as Exception);
        }
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            LogBuilder.Configuration(LogConfigXmlReader.Load($"{AppContext.BaseDirectory}DignusLog.config"));
            LogBuilder.Build();

            //Echo
            var sessionInitializer = new SessionConfiguration(EchoSetupFactory);

            sessionInitializer.SocketOption.SendBufferSize = 65536;
            sessionInitializer.SocketOption.MaxPendingSendBytes = int.MaxValue;

            var pfxPath = Path.Combine(AppContext.BaseDirectory, "server.pfx");
            X509Certificate2 serverCert = X509CertificateLoader.LoadPkcs12FromFile(pfxPath, "1234");

            var tlsOption = new TlsServerOptions(serverCert, initialSessionPoolSize: 100);
            EchoTlsServer echoServer = new(sessionInitializer, tlsOption);
            echoServer.Start(5000);
            LogHelper.Info($"start server... port : {5000}");
            Console.ReadKey();
        }
        static SessionSetup EchoSetupFactory()
        {
            EchoPacketProcessor echoPacketHandler = new();
            return new SessionSetup(
                    echoPacketHandler,
                    echoPacketHandler,
                    []);
        }
    }
}