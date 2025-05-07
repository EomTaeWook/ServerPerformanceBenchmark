using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proto;
using SuperSocketLite.Common;
using SuperSocketLite.SocketBase.Protocol;
using SuperSocketLite.SocketEngine.Protocol;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EchoServer;

///// <summary>
///// 이진 요청 정보 클래스
///// 패킷의 헤더와 보디에 해당하는 부분을 나타냅니다.
///// </summary>
//public class EFBinaryRequestInfo : BinaryRequestInfo
//{
//    /// <summary>
//    /// 전체 크기
//    /// </summary>
//    public Int16 TotalSize { get; private set; }

//    /// <summary>
//    /// 패킷 ID
//    /// </summary>
//    public Int16 PacketID { get; private set; }

//    /// <summary>
//    /// 예약(더미)값 
//    /// </summary>
//    public SByte Value1 { get; private set; }

//    /// <summary>
//    /// 헤더 크기
//    /// </summary>
//    public const int HeaderSize = 5;

//    /// <summary>
//    /// EFBinaryRequestInfo 클래스의 새 인스턴스를 초기화합니다.
//    /// </summary>
//    /// <param name="totalSize">전체 크기</param>
//    /// <param name="packetID">패킷 ID</param>
//    /// <param name="value1">값 1</param>
//    /// <param name="body">바디</param>
//    public EFBinaryRequestInfo(Int16 totalSize, Int16 packetID, SByte value1, byte[] body)
//        : base(null, body)
//    {
//        this.TotalSize = totalSize;
//        this.PacketID = packetID;
//        this.Value1 = value1;
//    }
//}

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
        return new ByteReqeustInfo(new ArraySegment<byte>(readBuffer, offset, length));
    }
}
