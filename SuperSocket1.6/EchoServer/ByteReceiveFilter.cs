using SuperSocketLite.SocketBase.Protocol;
using System;

namespace EchoServer;
public class ByteReqeustInfo : IRequestInfo<ArraySegment<byte>>
{
    public string Key => string.Empty; // 프로토버프 통신에서는 Key 필요 없음, 빈 문자열

    public ArraySegment<byte> Body { get; private set; }

    public ByteReqeustInfo(ArraySegment<byte> body)
    {
        this.Body = body;
    }
}

/// <summary>
/// 수신 필터 클래스
/// </summary>
public class ByteReceiveFilter : ReceiveFilterBase<ByteReqeustInfo>
{
    /// <summary>
    /// ReceiveFilter 클래스의 새 인스턴스를 초기화합니다.
    /// </summary>
    public ByteReceiveFilter()
    {
    }

    public override ByteReqeustInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
    {
        rest = 0;
        if (toBeCopied)
        {
            var copied = new byte[length];
            Buffer.BlockCopy(readBuffer, offset, copied, 0, length);
            return new ByteReqeustInfo(new ArraySegment<byte>(copied));
        }
        return new ByteReqeustInfo(new ArraySegment<byte>(readBuffer, offset, length));
    }
}
