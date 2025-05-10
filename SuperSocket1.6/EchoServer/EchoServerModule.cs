using SuperSocketLite.SocketBase;
using SuperSocketLite.SocketBase.Config;
using SuperSocketLite.SocketBase.Logging;
using SuperSocketLite.SocketBase.Protocol;
using SuperSocketLite.SocketEngine;
using System;


namespace EchoServer;

/// <summary>
/// 네트워크 세션 클래스입니다.
/// </summary>
internal class EchoNetworkSession : AppSession<EchoNetworkSession, ByteReqeustInfo>
{
}

/// <summary>
/// 메인 서버 클래스입니다.
/// </summary>
internal class EchoServerModule : AppServer<EchoNetworkSession, ByteReqeustInfo>
{
    /// <summary>
    /// 메인 로거 인스턴스입니다.
    /// </summary>
    public static ILog s_MainLogger;

    private IServerConfig _config;
    private bool _isRun = false;

    /// <summary>
    /// MainServer 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public EchoServerModule()
        : base(new DefaultReceiveFilterFactory<ByteReceiveFilter, ByteReqeustInfo>())
    {
        NewSessionConnected += new SessionHandler<EchoNetworkSession>(OnConnected);
        SessionClosed += new SessionHandler<EchoNetworkSession, CloseReason>(OnClosed);
        NewRequestReceived += new RequestHandler<EchoNetworkSession, ByteReqeustInfo>(RequestReceived);
    }

    /// <summary>
    /// 핸들러를 등록합니다.
    /// </summary>
    private void RegistHandler()
    {

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
            Name = option.Name,
            MaxRequestLength = 8192,
            ListenBacklog = 200,
        };
    }

    /// <summary>
    /// 서버를 생성합니다.
    /// </summary>
    public void CreateServer()
    {
        try
        {
            bool isResult = base.Setup(_config, socketServerFactory: new SocketServerFactory(), logFactory: null);

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
    private void OnConnected(EchoNetworkSession session)
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
    private void OnClosed(EchoNetworkSession session, CloseReason reason)
    {
        //s_MainLogger.Info($"[{DateTime.Now}] 세션 번호 {session.SessionID},  접속해제: {reason.ToString()}");
    }

    /// <summary>
    /// 클라이언트로부터 요청을 받았을 때 호출됩니다.
    /// </summary>
    /// <param name="session">요청을 받은 세션</param>
    /// <param name="reqInfo">받은 요청 정보</param>
    private void RequestReceived(EchoNetworkSession session, ByteReqeustInfo reqInfo)
    {
        session.Send(reqInfo.Body);
    }
}


