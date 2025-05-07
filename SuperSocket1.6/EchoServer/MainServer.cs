using System;
using System.Collections.Generic;
using System.Threading;

using SuperSocketLite.SocketBase.Logging;
using SuperSocketLite.SocketBase;
using SuperSocketLite.SocketBase.Protocol;
using SuperSocketLite.SocketBase.Config;
using Proto;
using Google.Protobuf;
using SuperSocketLite.SocketEngine;
using EchoServer.Handler;


namespace EchoServer;

/// <summary>
/// 메인 서버 클래스입니다.
/// </summary>
class MainServer : AppServer<NetworkSession, ReqeustInfo>
{
    /// <summary>
    /// 메인 로거 인스턴스입니다.
    /// </summary>
    public static ILog s_MainLogger;

    private IServerConfig _config;
    private bool _isRun = false;

	private Dictionary<C2S.BodyOneofCase, Action<NetworkSession, C2S>> _handlers = new();

	/// <summary>
	/// MainServer 클래스의 새 인스턴스를 초기화합니다.
	/// </summary>
	public MainServer()
        : base(new DefaultReceiveFilterFactory<ReceiveFilter, ReqeustInfo>())
    {
        NewSessionConnected += new SessionHandler<NetworkSession>(OnConnected);
        SessionClosed += new SessionHandler<NetworkSession, CloseReason>(OnClosed);
        NewRequestReceived += new RequestHandler<NetworkSession, ReqeustInfo>(RequestReceived);
    }

    /// <summary>
    /// 핸들러를 등록합니다.
    /// </summary>
    private void RegistHandler()
    {
		_handlers[C2S.BodyOneofCase.Login] = (s, p) =>
		{
			DummyProtocolHandler.Process(s, p.Login);
		};

		_handlers[C2S.BodyOneofCase.JoinRoom] = (s, p) =>
		{
			DummyProtocolHandler.Process(s, p.JoinRoom);
		};
	}

    /// <summary>
    /// 서버 설정을 초기화합니다.
    /// </summary>
    /// <param name="option">서버 옵션</param>
    public void InitConfig(ServerOption option)
    {
        _config = new ServerConfig
        {
            Port = option.Port,
            Ip = "Any",
            MaxConnectionNumber = 10000,
            Mode = SocketMode.Tcp,
            Name = option.Name
        };
    }

    /// <summary>
    /// 서버를 생성합니다.
    /// </summary>
    public void CreateServer()
    {
        try
        {
            bool isResult = base.Setup(_config, socketServerFactory: new SocketServerFactory());

            if (isResult == false)
            {
                Console.WriteLine("[ERROR] 서버 네트워크 설정 실패 ㅠㅠ");
                return;
            }

            RegistHandler();

            _isRun = true;
        }
        catch (Exception ex)
        {
            //s_MainLogger.Error($"서버 생성 실패: {ex.ToString()}");
        }
    }

    /// <summary>
    /// 서버를 종료합니다.
    /// </summary>
    public void Destory()
    {
        base.Stop();

        _isRun = false;
    }


    /// <summary>
    /// 서버가 실행 중인지 확인합니다.
    /// </summary>
    /// <param name="curState">현재 서버 상태</param>
    /// <returns>서버가 실행 중인지 여부</returns>
    public bool IsRunning(ServerState curState)
    {
        if (curState == ServerState.Running)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 클라이언트가 접속했을 때 호출됩니다.
    /// </summary>
    /// <param name="session">접속한 세션</param>
    private void OnConnected(NetworkSession session)
    {
        //s_MainLogger.Debug($"[{DateTime.Now}] 세션 번호 {session.SessionID} 접속 start, ThreadId: {System.Threading.Thread.CurrentThread.ManagedThreadId}");

        //Thread.Sleep(3000);
        //MainLogger.Info($"세션 번호 {session.SessionID} 접속 end, ThreadId: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
    }

    /// <summary>
    /// 클라이언트가 접속을 해제했을 때 호출됩니다.
    /// </summary>
    /// <param name="session">해제된 세션</param>
    /// <param name="reason">해제 사유</param>
    private void OnClosed(NetworkSession session, CloseReason reason)
    {
        //s_MainLogger.Info($"[{DateTime.Now}] 세션 번호 {session.SessionID},  접속해제: {reason.ToString()}");
    }

    /// <summary>
    /// 클라이언트로부터 요청을 받았을 때 호출됩니다.
    /// </summary>
    /// <param name="session">요청을 받은 세션</param>
    /// <param name="reqInfo">받은 요청 정보</param>
    private void RequestReceived(NetworkSession session, ReqeustInfo reqInfo)
    {
        _handlers[reqInfo.Body.BodyCase](session, reqInfo.Body);
    }
}


/// <summary>
/// 네트워크 세션 클래스입니다.
/// </summary>
public class NetworkSession : AppSession<NetworkSession, ReqeustInfo>
{
}
