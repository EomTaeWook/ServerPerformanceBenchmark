using System;

namespace EchoServer;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello SuperSocketLite");

        //// 명령어 인수로 서버 옵셥을 받아서 처리한다.
        //var serverOption = ParseCommandLine(args);
        //if (serverOption == null)
        //{
        //    return;
        //}

        // 서버를 생성하고 초기화한다.
        var server = new EchoServerModule();
        server.InitConfig(new ServerOption()
        {
            Port = 5000,
            Name = "localhost",
            MaxConnectionNumber = 1000

        });
        server.CreateServer();

        // 서버를 시작한다.
        var IsResult = server.Start();

        if (IsResult)
        {
            Console.WriteLine("start");
            //MainServer.s_MainLogger.Info("서버 네트워크 시작");
        }
        else
        {
            Console.WriteLine("서버 네트워크 시작 실패");
            return;
        }


        Console.WriteLine("key를 누르면 종료한다....");
        Console.ReadKey();

        server.Destory();
    }

    static ServerOption ParseCommandLine(string[] args)
    {
        var option = new ServerOption
        {
            Port = 5000,
            MaxConnectionNumber = 1000,
            Name = "EchoServer",
        };

        return option;
    }

}

public class ServerOption
{
    public int Port { get; set; } = 5000;

    public int MaxConnectionNumber { get; set; } = 1000;

    public string Name { get; set; }
}
